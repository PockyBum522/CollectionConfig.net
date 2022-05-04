namespace CollectionConfig.net.Common.Logic.Interfaces;

/// <summary>
/// Reads data from a specified CSV file
/// </summary>
public interface IFileReader
{
    /// <summary>
    /// Reads data in from file
    /// </summary>
    /// <param name="fullPathToFile">Full path to the file to read</param>
    /// <returns>All contents of the file as one string</returns>
    string Read(string fullPathToFile);
}