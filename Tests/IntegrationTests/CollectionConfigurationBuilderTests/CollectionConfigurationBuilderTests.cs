using System;
using System.Collections.Generic;
using System.IO;
using CollectionConfig.net.Logic.CacheLoaders;
using CollectionConfig.net.Logic.StorageReaders.File;
using CollectionConfig.net.Logic.StorageWriters.File;
using CollectionConfig.net.Main;
using FluentAssertions;
using IntegrationTests.TestResources.ExamplePerson;
using NUnit.Framework;
using Serilog;

namespace IntegrationTests.CollectionConfigurationBuilderTests;

public class CollectionConfigurationBuilderTests
{
    private const string ExampleOriginalPersonListCsvPath = @"TestResources\ExamplePerson\ExamplePersonList.csv";

    // ReSharper disable once FieldCanBeMadeReadOnly.Local because if we uncomment logging below it won't be readonly
    private ILogger? _testLogger = null;

    [SetUp]
    public void SetUp()
    {
        // Uncomment these if you want logging. Debugging seems potentially iffy on proxy objects, so logging may help:
        
        // var logPath =
        //     Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CollectionConfig.net.log"); 
        //
        // _testLogger ??= TestLogger.GetTestLogger(logPath);
        
        // Ensure file is there
        File.Exists(ExampleOriginalPersonListCsvPath).Should().Be(true);
    }
    
    [Test]
    public void CollectionConfigurationBuilder_WhenBuiltWithCsvFile_ShouldNotBeNull()
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExampleOriginalPersonListCsvPath)
                .UseLogger(_testLogger!)
                .Build();

        myList.Should().NotBeNull();
    }

    [Test]
    public void CollectionConfigurationBuilder_WhenBuiltWithCsvFileAndCustomStorageInOrder01_ShouldThrowException()
    {
        var reader = new FileStoreReader(ExampleOriginalPersonListCsvPath);
        var writer = new CsvFileWriter(ExampleOriginalPersonListCsvPath);
        var cacheLoader = new CsvCacheLoader(reader);
        
        var action =
            () => new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExampleOriginalPersonListCsvPath)
                .UseCustomStorage(reader, writer, cacheLoader)
                .UseLogger(_testLogger!)
                .Build();

        action.Should().Throw<Exception>();
    }
    
    [Test]
    public void CollectionConfigurationBuilder_WhenBuiltWithCsvFileAndCustomStorageInOrder02_ShouldThrowException()
    {
        var reader = new FileStoreReader(ExampleOriginalPersonListCsvPath);
        var writer = new CsvFileWriter(ExampleOriginalPersonListCsvPath);
        var cacheLoader = new CsvCacheLoader(reader);
        
        var action =
            () => new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCustomStorage(reader, writer, cacheLoader)
                .UseCsvFile(ExampleOriginalPersonListCsvPath)
                .UseLogger(_testLogger!)
                .Build();

        action.Should().Throw<Exception>();
    }
    
    [Test]
    public void CollectionConfigurationBuilder_WhenBuiltWithCustomStorage_ShouldBuild()
    {
        var reader = new FileStoreReader(ExampleOriginalPersonListCsvPath);
        var writer = new CsvFileWriter(ExampleOriginalPersonListCsvPath);
        var cacheLoader = new CsvCacheLoader(reader);
        
        var action =
            () => new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCustomStorage(reader, writer, cacheLoader)
                .UseLogger(_testLogger!)
                .Build();

        action.Should().NotThrow<Exception>();
    }
}
