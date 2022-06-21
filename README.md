# CollectionConfig.net

This is not associated with Config.net, I just liked how they set things up. My goal is to be able to do something similar with collections, especially being able to write them.

# Goal

The purpose of this project is that you should be able to read and write a native IList<ICustomInterface> object that maps to a JSON or CSV file on disk, easily.

# Current Progress

    CSV Files: 
        Read-Only functional
        Write support functional
        Remove item functional

    JSON Files: 
        Read-Only functional
    

# Usage, CSV: 

<ul>
    Reference the library and then:

    Set up an interface such as:

        /// <summary>
        /// Example person model, used for testing
        /// </summary>
        public interface IExamplePerson
        {
            string Name { get; set; }
            int Age { get; set; }
            double MilesRun { get; set; }
            string PetsName { get; set; }
        }

    Then set up a ExamplePersonList.csv file such as:

        Name,Age,MilesRun,PetsName
        David,32,3.15,Whiskey
        Alyssa,26,2.12,Maxx
        Robin,28,1.23,Snuggles
        Dyamond,31,2.23,Trixie

    (Headers must match the interface property names exactly. It is case sensitive.)

    Then to use it, you simply:

        var myList = 
                new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                    .UseCsvFile(ExamplePersonListCsvPath)
                    .Build();

        var result = myList[0].Name; // Rows index does not account for the header row, just rows with values
        
        result.Should().Be("David");

    And since it's an interface, injection is easy should you choose to go that route.
</ul>

# Usage, JSON:

<ul>
Using the same IExamplePerson Interface as the CSV uses:

JSON file should look like:

    [
        {
            "Name": "David",
            "Age": 32,
            "MilesRun": 3.15,
            "PetsName": "Whiskey"
        },
        {
            "Name": "Alyssa",
            "Age": 26,
            "MilesRun": 2.12,
            "PetsName": "Maxx"
        },
        {
            "Name": "Robin",
            "Age": 28,
            "MilesRun": 1.23,
            "PetsName": "Snuggles"
        },
        {
            "Name": "Dyamond",
            "Age": 31,
            "MilesRun": 2.23,
            "PetsName": "Trixie"
        }
    ]

Should support the same types that the CSV does.
</ul>

# Contributing: 

<ul>
More types can be added to this list in InterfaceInterceptor.SetInvocationReturnToValueAsSpecificType(). 
If you do so, please add tests for those types to the integration tests for CSV and JSON both. This means updading the example interface, and JSON and CSV example files to match.
</ul>