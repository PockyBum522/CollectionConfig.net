# CollectionConfig.net

This is not associated with Config.net, I just liked how they set things up. My goal is to be able to do something similar with collections, especially being able to write them.

# Goal

IT READS!!

Currently only supports CSV. To use, reference the library and then:

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

Then to use it, you simply:

    var myList = 
            new CollectionConfigurationBuilder<IList<IExamplePerson>>()
                .UseCsvFile(ExamplePersonListCsvPath)
                .Build();

        var result = myList[0].Name;
        
        result.Should().Be("David");