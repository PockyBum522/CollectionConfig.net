namespace CollectionConfig.net.Core.Interfaces;

/// <summary>
/// For objects that can write formatted elements to a configuration file that represents the List of ICustomInterface
/// </summary>
public interface IFileWriter
{
    /// <summary>
    /// Gets a new element of List of ICustomInterface and formats it how it will be saved in the configuration file
    /// that represents the List of ICustomInterface
    /// </summary>
    /// <returns>Formatted string identical to how a new element looks in the configuration file that represents the
    /// List of ICustomInterface</returns>
    string FormatNewElement(object elementToFormatForFile);

    /// <summary>
    /// Handles actually saving a new element to the configuration file that represents the List of ICustomInterface
    /// </summary>
    /// <param name="formattedNewElement">Already formatted new element</param>
    void WriteNewElement(string formattedNewElement);
}