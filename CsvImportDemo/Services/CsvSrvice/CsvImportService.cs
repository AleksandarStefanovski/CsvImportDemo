using CsvHelper;
using CsvHelper.Configuration;
using CsvImportDemo.Models;
using FluentValidation;
using MongoDB.Driver;
using System.Collections.Concurrent;

namespace CsvImportDemo.Services.CsvSrvice;

public class CsvImportService : ICsvImportService<Person>
{
    private readonly IMongoCollection<Person> _userCollection;
    private readonly IValidator<Person> _personValidator;

    public CsvImportService(IMongoDatabase database, IValidator<Person> personValidator)
    {
        _userCollection = database.GetCollection<Person>("Persons");
        _personValidator = personValidator;
    }

    public void ImportFromCsv(string filePath, CsvImportOptions options)
    {
        using var reader = new StreamReader(filePath, options.FileEncoding);
        using var csv = new CsvReader(reader, new CsvConfiguration(options.Culture)
        {
            HasHeaderRecord = options.HasHeaderRecord
        });
        csv.Context.RegisterClassMap<PersonMap>();

        int batchSize = 1000;
        var buffer = new List<Person>(batchSize);

        foreach (var user in csv.GetRecords<Person>())
        {
            buffer.Add(user);

            if (buffer.Count == batchSize)
            {
                ProcessBatch(buffer);
                buffer.Clear();
            }
        }

        if (buffer.Count > 0)
        {
            ProcessBatch(buffer);
        }
    }

    private void ProcessBatch(IEnumerable<Person> batch)
    {
        var validRecords = ValidateRecords(batch);
        InsertRecords(validRecords);
    }

    private IEnumerable<Person> ValidateRecords(IEnumerable<Person> batch)
    {
        var validRecords = new ConcurrentBag<Person>();

        Parallel.ForEach(batch, user =>
        {
            var validationResult = _personValidator.Validate(user);
            if (validationResult.IsValid)
            {
                validRecords.Add(user);
            }
            else
            {
                Console.WriteLine($"Record with ID {user.Id} is invalid:");
                foreach (var failure in validationResult.Errors)
                {
                    Console.WriteLine($" - {failure.PropertyName}: {failure.ErrorMessage}");
                }
            }
        });

        return validRecords;
    }

    private void InsertRecords(IEnumerable<Person> validRecords)
    {
        var bulkOps = new List<WriteModel<Person>>();

        foreach (var user in validRecords)
        {
            var filter = Builders<Person>.Filter.Eq(x => x.Id, user.Id);
            var update = Builders<Person>.Update
                .Set(x => x.FirstName, user.FirstName)
                .Set(x => x.LastName, user.LastName)
            .Set(x => x.Address, user.Address);

            bulkOps.Add(new UpdateOneModel<Person>(filter, update) { IsUpsert = true });
        }

        if (bulkOps.Count > 0)
        {
            _userCollection.BulkWrite(bulkOps);
        }
    }
}