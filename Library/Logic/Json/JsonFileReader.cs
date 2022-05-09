using CollectionConfig.net.Core.Interfaces;

namespace CollectionConfig.net.Logic.Json;

/// <summary>
/// Reads data in from CSV file
/// </summary>
public class JsonFileReader : IFileReader
{
    /// <summary>
    /// Reads data in from csv file
    /// </summary>
    /// <param name="fullPathToFile">Full path to the CSV file to read</param>
    /// <returns>All contents of the CSV file as one string</returns>
    public string Read(string fullPathToFile)
    {
        return File.ReadAllText(fullPathToFile);
    }
}