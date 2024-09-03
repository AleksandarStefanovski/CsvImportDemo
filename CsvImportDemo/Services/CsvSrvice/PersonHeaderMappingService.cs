using CsvHelper.Configuration;
using CsvImportDemo.Models;

namespace CsvImportDemo.Services.CsvSrvice;

public class PersonMap : ClassMap<Person>
{
    public PersonMap()
    {
        Map(m => m.Id).Name("Id");
        Map(m => m.FirstName).Name("FirstName");
        Map(m => m.LastName).Name("LastName");
        Map(m => m.Email).Name("Email");
        Map(m => m.PhoneNumber).Name("PhoneNumber");
        Map(m => m.Address).Name("Address");
    }
}