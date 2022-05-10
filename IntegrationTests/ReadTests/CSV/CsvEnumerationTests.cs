using System;
using System.Collections.Generic;
using System.IO;
using CollectionConfig.net.Main;
using FluentAssertions;
using IntegrationTests.TestResources.ExamplePerson;
using NUnit.Framework;

namespace IntegrationTests.ReadTests.CSV;

public class CsvEnumerationTests
{
    private const string ExamplePersonListCsvPath = @"TestResources\ExamplePerson\ExamplePersonList.csv";
    
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
                .UseCsvFile(ExamplePersonListCsvPath).Build();

        myList.Should().NotBeNull();
    }
    
    [Test]
    public void CollectionConfiguration_StringsWhenAccessed_NamesShouldBeCorrect()
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath)
                .Build();

        var result = myList.Count;
        
        result.Should().Be(4);
    }
}
