using CollectionConfig.net.Core.Interfaces;

namespace CollectionConfig.net.Logic.Writers;

/// <summary>
/// Used to format JSON data and write that data to a configuration file or initialize new configuration file if necessary
/// </summary>
public class JsonFileWriter : IFileWriter
{
    /// <summary>
    /// Constructor for injecting dependencies
    /// </summary>
    /// <param name="configurationFilePath">Full path to the configuration file, or where it will be</param>
    public JsonFileWriter(string configurationFilePath)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets a new element of List of ICustomInterface and formats it how it will be saved in the configuration file
    /// that represents the List of ICustomInterface
    /// </summary>
    /// <returns>Formatted string identical to how a new element looks in the configuration file that represents the
    /// List of ICustomInterface</returns>
    public string FormatNewElement(object elementToFormatForFile)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Handles actually saving a new element to the configuration file that represents the List of ICustomInterface
    /// </summary>
    /// <param name="formattedNewElement">Already formatted new element</param>
    public void WriteNewElement(string formattedNewElement)
    {
        throw new NotImplementedException();
    }
}