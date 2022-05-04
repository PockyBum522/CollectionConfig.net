using System.Collections.Generic;
using JetBrains.Annotations;

namespace CollectionConfig.net.IntegrationTests.TestResources.ExamplePerson;

[PublicAPI]
public class ExampleListOfPersonNativeFilledTestObject
{
    public List<PersonForTest> People { get; } = new();

    public ExampleListOfPersonNativeFilledTestObject()
    {
        People.Add(new PersonForTest(){Name="David", Age = 32, MilesRun = 3.15, PetsName = "Whiskey"});
        People.Add(new PersonForTest(){Name="Alyssa", Age = 26, MilesRun = 2.12, PetsName = "Maxx"});
        People.Add(new PersonForTest(){Name="Robin", Age = 28, MilesRun = 1.23, PetsName = "Snuggles"});
        People.Add(new PersonForTest(){Name="Dyamond", Age = 31, MilesRun = 2.23, PetsName = "Trixie"});
    }
}

/// <summary>
/// This is only used in the NativeFilledTestObject class
/// </summary>
public class PersonForTest
{
    /// <summary>
    /// Person's name. Only used in testing
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// Person's age to test integers. Only used in testing
    /// </summary>
    public int Age { get; set; }
    
    /// <summary>
    /// Miles Run to test doubles. Only used in testing
    /// </summary>
    public double MilesRun { get; set; }
    
    /// <summary>
    /// Person's pet's name. Only used in testing
    /// </summary>
    public string PetsName { get; set; } = "";
}