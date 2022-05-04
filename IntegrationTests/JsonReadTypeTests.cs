using System.Collections.Generic;
using System.IO;
using CollectionConfig.net.Common;
using CollectionConfig.net.IntegrationTests.TestResources.ExamplePerson;
using FluentAssertions;
using NUnit.Framework;

namespace CollectionConfig.net.IntegrationTests;

public class JsonReadTypeTests
{
    private const string ExamplePersonListJsonPath = @"TestResources\ExamplePerson\ExamplePersonList.json";
    
    [Test]
    public void ThisTest_WhenStarted_ResourcesJsonShouldBePresent()
    {
        var result = File.Exists(ExamplePersonListJsonPath);
    
        result.Should().Be(true);
    }
    
    [Test]
    public void CollectionConfigurationBuilder_WhenBuiltWithJsonFile_ShouldNotBeNull()
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseJsonFile(ExamplePersonListJsonPath).Build();

        myList.Should().NotBeNull();
    }
    
    [Test]
    [TestCase(0, "David")]
    [TestCase(1, "Alyssa")]
    [TestCase(2, "Robin")]
    [TestCase(3, "Dyamond")]
    public void JsonCollectionConfiguration_StringsWhenAccessed_NamesShouldBeCorrect(int index, string expected)
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseJsonFile(ExamplePersonListJsonPath)
                .Build();

        var result = myList[index].Name;
        
        result.Should().Be(expected);
    }    
    
    [Test]
    [TestCase(0, 32)]
    [TestCase(1, 26)]
    [TestCase(2, 28)]
    [TestCase(3, 31)]
    public void JsonCollectionConfiguration_IntsWhenAccessed_AgesShouldBeCorrect(int index, int expected)
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseJsonFile(ExamplePersonListJsonPath).Build();
    
        var result = myList[index].Age;
        
        result.Should().Be(expected);
    }
    
    [Test]
    [TestCase(0, 3.15)]
    [TestCase(1, 2.12)]
    [TestCase(2, 1.23)]
    [TestCase(3, 2.23)]
    public void JsonCollectionConfiguration_DoublesWhenAccessed_MilesRunShouldBeCorrect(int index, double expected)
    {
        var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseJsonFile(ExamplePersonListJsonPath).Build();
    
        var result = myList[index].MilesRun;
        
        result.Should().Be(expected);
    }
}
