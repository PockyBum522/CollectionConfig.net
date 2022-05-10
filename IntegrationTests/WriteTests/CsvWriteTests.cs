using System;
using System.Collections.Generic;
using System.IO;
using CollectionConfig.net.Main;
using FluentAssertions;
using IntegrationTests.TestResources.ExamplePerson;
using NUnit.Framework;
using Serilog;

namespace IntegrationTests.WriteTests;

public class CsvWriteTests
{
    private const string ExamplePersonListCsvPath = @"TestResources\ExamplePerson\ExamplePersonList_WRITE_TEST.csv";
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
        
        // Reset example file to original state
        File.Delete(ExamplePersonListCsvPath);
        
        // Ensure file isn't locked open or something (ensure was deleted successfully)
        File.Exists(ExamplePersonListCsvPath).Should().Be(false);
    
        File.Copy(ExampleOriginalPersonListCsvPath, ExamplePersonListCsvPath);
        
        // Ensure file is there
        File.Exists(ExamplePersonListCsvPath).Should().Be(true);
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
    public void IListGenerateNewElementExtensionMethod_WhenCalled_ShouldGenerateBlankElementProxy()
    {
        var myList =
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath)
                .UseLogger(_testLogger!)
                .Build();

        var personToAdd = myList.GetNewElement();
    
        personToAdd.Name.Should().BeEmpty();
        personToAdd.Age.Should().Be(0);
        personToAdd.MilesRun.Should().Be(0.0);
        personToAdd.PetsName.Should().BeEmpty();
    }
    
    [Test]
    public void IListGenerateNewElementExtensionMethodObject_WhenWrittenTo_ShouldSaveData()
    {
        var myList =
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath)
                .UseLogger(_testLogger!)
                .Build();

        var personToAdd = myList.GetNewElement();
        
        // Now write to the object
        personToAdd.Name = "Jackson";
        personToAdd.Age = 50;
        personToAdd.MilesRun = 26.2;
        
        // Now test
        personToAdd.Name.Should().Be("Jackson");
        personToAdd.Age.Should().Be(50);
        personToAdd.MilesRun.Should().Be(26.2);
        personToAdd.PetsName.Should().BeEmpty();
    }
    
    [Test]
    public void CollectionConfiguration_StringsWhenAdded_ShouldBeAddedToFile()
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath)
                .UseLogger(_testLogger!)
                .Build();

        var personToAdd = myList.GetNewElement();

        personToAdd.Name = "Hades";
        personToAdd.Age = 97;
        personToAdd.MilesRun = 907.32;
        personToAdd.PetsName = "Cerberus";
        
        myList.Add(personToAdd);
        
        var result = File.ReadAllText(ExamplePersonListCsvPath);
        
        result.Should().Be(
            @"Name,Age,MilesRun,PetsName
David,32,3.15,Whiskey
Alyssa,26,2.12,Maxx
Robin,28,1.23,Snuggles
Dyamond,31,2.23,Trixie
Hades,97,907.32,Cerberus");
    } 
    
    [Test]
    public void CollectionConfiguration_ElementsWhenAddedTwice_ShouldBeAddedToFile()
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath)
                .UseLogger(_testLogger!)
                .Build();

        var personToAdd = myList.GetNewElement();

        personToAdd.Name = "Hades";
        personToAdd.Age = 97;
        personToAdd.MilesRun = 907.32;
        personToAdd.PetsName = "Cerberus";
        
        var personToAddTwo = myList.GetNewElement();

        personToAddTwo.Name = "Timothy";
        personToAddTwo.Age = 22;
        personToAddTwo.MilesRun = 7.32;
        personToAddTwo.PetsName = "Fido";
        
        myList.Add(personToAdd);
        myList.Add(personToAddTwo);
        
        var result = File.ReadAllText(ExamplePersonListCsvPath);
        
        result.Should().Be(
            @"Name,Age,MilesRun,PetsName
David,32,3.15,Whiskey
Alyssa,26,2.12,Maxx
Robin,28,1.23,Snuggles
Dyamond,31,2.23,Trixie
Hades,97,907.32,Cerberus
Timothy,22,7.32,Fido");
    } 
    
    [Test]
    public void CollectionConfiguration_ElementsWhenAddedTwiceInDifferentSequence_ShouldBeAddedToFile()
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath)
                .UseLogger(_testLogger!)
                .Build();

        var personToAdd = myList.GetNewElement();

        personToAdd.Name = "Hades";
        personToAdd.Age = 97;
        personToAdd.MilesRun = 907.32;
        personToAdd.PetsName = "Cerberus";
        
        myList.Add(personToAdd);
        
        var personToAddTwo = myList.GetNewElement();

        personToAddTwo.Name = "Timothy";
        personToAddTwo.Age = 22;
        personToAddTwo.MilesRun = 7.32;
        personToAddTwo.PetsName = "Fido";
        
        myList.Add(personToAddTwo);
        
        var result = File.ReadAllText(ExamplePersonListCsvPath);
        
        result.Should().Be(
            @"Name,Age,MilesRun,PetsName
David,32,3.15,Whiskey
Alyssa,26,2.12,Maxx
Robin,28,1.23,Snuggles
Dyamond,31,2.23,Trixie
Hades,97,907.32,Cerberus
Timothy,22,7.32,Fido");
    } 
    
    [Test]
    public void CollectionConfiguration_ElementsWhenAddedTwiceWithSameObject_ShouldBeAddedToFile()
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath)
                .UseLogger(_testLogger!)
                .Build();

        var personToAdd = myList.GetNewElement();

        personToAdd.Name = "Hades";
        personToAdd.Age = 97;
        personToAdd.MilesRun = 907.32;
        personToAdd.PetsName = "Cerberus";
        
        myList.Add(personToAdd);

        personToAdd = myList.GetNewElement();

        personToAdd.Name = "Timothy";
        personToAdd.Age = 22;
        personToAdd.MilesRun = 7.32;
        personToAdd.PetsName = "Fido";
        
        myList.Add(personToAdd);
        
        var result = File.ReadAllText(ExamplePersonListCsvPath);
        
        result.Should().Be(
            @"Name,Age,MilesRun,PetsName
David,32,3.15,Whiskey
Alyssa,26,2.12,Maxx
Robin,28,1.23,Snuggles
Dyamond,31,2.23,Trixie
Hades,97,907.32,Cerberus
Timothy,22,7.32,Fido");
    } 
    
    [Test]
    public void CollectionConfiguration_OnRemove_ShouldRemoveItemFromFile()
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath)
                .UseLogger(_testLogger!)
                .Build();

        foreach (var item in myList)
        {
            if (item.Name != "Robin") continue;
            
            // Otherwise, remove matching
            myList.Remove(item);
            break;
        }
        
        var result = File.ReadAllText(ExamplePersonListCsvPath);
        
        result.Should().Be(
@"Name,Age,MilesRun,PetsName
David,32,3.15,Whiskey
Alyssa,26,2.12,Maxx
Dyamond,31,2.23,Trixie");
    } 
}
