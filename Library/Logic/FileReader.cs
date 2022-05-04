using System.IO;
using CollectionConfig.net.Common.Logic.Interfaces;

namespace CollectionConfig.net.Common.Logic.Json;

/// <summary>
/// Reads data in from CSV file
/// </summary>
public class FileReader
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