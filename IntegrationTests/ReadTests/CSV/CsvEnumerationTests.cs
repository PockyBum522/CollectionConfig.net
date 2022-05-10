using System;
using System.Collections.Generic;
using System.IO;
using CollectionConfig.net.Main;
using FluentAssertions;
using IntegrationTests.TestResources.ExamplePerson;
using NUnit.Framework;
using Serilog;

namespace IntegrationTests.ReadTests.CSV;

public class CsvEnumerationTests
{
    private const string ExamplePersonListCsvPath = @"TestResources\ExamplePerson\ExamplePersonList.csv";

    // ReSharper disable once FieldCanBeMadeReadOnly.Local because if we uncomment logging below it won't be readonly
    private ILogger? _testLogger = null;
    
    [SetUp]
    public void SetUp()
    {
        // Uncomment these if you want logging. Debugging seems potentially iffy on proxy objects, so logging may help:
        
        var logPath =
            Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CollectionConfig.CsvEnumerationTests.net.log"); 
        
        _testLogger ??= TestLogger.GetTestLogger(logPath);
    }
    
    [Test]
    public void ThisTest_WhenStarted_ResourcesCsvShouldBePresent()
    {
        var result = File.Exists(ExamplePersonListCsvPath);
    
        result.Should().Be(true);
    }
    
    [Test]
    public void CollectionConfigurationBuilder_WhenBuiltWithCsvFile_ShouldNotBeNull()
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath)
                .UseLogger(_testLogger!)
                .Build();

        myList.Should().NotBeNull();
    }
    
    [Test]
    public void CollectionConfiguration_CountWhenAccessed_ShouldBe4()
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath)
                .UseLogger(_testLogger!)
                .Build();

        var result = myList.Count;
        
        result.Should().Be(4);
    }
    
    [Test]
    public void CollectionConfiguration_ForEachWhenAccessed_NamesShouldBeCorrect()
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath)
                .UseLogger(_testLogger!)
                .Build();

        var accumulator = "";
        
        foreach (var item in myList)
        {
            accumulator += item.Name;
        }
        
        accumulator.Should().Be("test");
    }
}
