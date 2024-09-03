using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using CsvImportBenchmarkDemo.Services;

var config = ManualConfig.Create(DefaultConfig.Instance)
    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
    .AddLogger(ConsoleLogger.Default)
    .AddJob(Job.Default
            .WithWarmupCount(2)
            .WithIterationCount(2)
            .WithMinIterationCount(2)
            .WithMaxIterationCount(3)
            .WithLaunchCount(1)
        );

// Run the benchmarks
var summary = BenchmarkRunner.Run<CsvImportBenchmarkService>(config);

// check the docker container logs for CsvImportBenchmarkDemo results, or just search in the solution explorer for the folder "BencharkDotNet.Artifacts" to see exports