using System.IO;
using CollectionConfig.net.Common.Logic.Csv.Interfaces;

namespace CollectionConfig.net.Common.Logic.Csv;

/// <summary>
/// Reads data in from CSV file
/// </summary>
public class FileFileReader : IFileReader
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