using System.IO;
using CollectionConfig.net.IntegrationTests.TestResources.ExamplePerson;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CollectionConfig.net.IntegrationTests;

public class UtilityJsonWriter
{
    private string ExamplePersonListJsonPath => @"D:\Dropbox\Documents\Desktop\ExamplePersonList_TESTONLY.json";

    private bool EnableExampleFileWrite = false;
    
    [Test]
    public void Utility_WhenRun_WriteExampleJsonFileFromNativeFilledObject()
    {
        // Only run this if it's enabled. This test is just to dump a native C object to JSON for testing
        // and it should normally remain disabled
        if (EnableExampleFileWrite)
        {
            var filledNativeObject = new ExampleListOfPersonNativeFilledTestObject();

            var jsonString = JsonConvert.SerializeObject(filledNativeObject, Formatting.Indented);
        
            File.Delete(ExamplePersonListJsonPath);
        
            File.WriteAllText(ExamplePersonListJsonPath, jsonString);    
        }

        true.Should().Be(true);
    }
}