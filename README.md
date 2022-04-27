# CollectionConfig.net

This is not associated with Config.net, I just liked how they set things up. My goal is to be able to do something similar with collections, especially being able to write them.

# Goal

The purpose of this project is that you should be able to do this:

namespace CollectionConfig.net.Interfaces
{
    public interface IConfigurationExample
    {
        public IList<PersonConfiguration> MyListOfPeople { get; set; }
    }
}

namespace CollectionConfig.net.Models
{
    public class PersonConfiguration
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}

and then this:

var myConfig = 
            new CollectionConfigurationBuilder<IConfigurationExample>()
                .UseCsvFile(@"C:\Users\Public\Documents\MyConfig.csv")
                .Build();

And then this:

    Console.WriteLine(
        myConfig.MyListOfPeople[0].Name);

    Which should read the current first value out of the file on disk and display it in the console

And also this:

    myConfig.MyListOfPeople.Add(new PersonConfiguration("Ted", 34));

    Which should immediately update the config file on disk to have another entry of "New value"