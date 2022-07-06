using CollectionConfig.net.Core.Interfaces;

namespace CollectionConfig.net.Logic.StorageReaders.File;

/// <summary>
/// Reads data in from CSV file
/// </summary>
public class FileReader : IDataStoreReader
{
    private readonly string _filePath;

    /// <param name="fullPathToFile">The full path to the file to read</param>
    public FileReader(string fullPathToFile)
    {
        _filePath = fullPathToFile;
    }
    
    /// <summary>
    /// Reads data in
    /// </summary>
    /// <returns>All contents of the file as one string</returns>
    public string Read()
    {
        return System.IO.File.ReadAllText(_filePath);
    }
}