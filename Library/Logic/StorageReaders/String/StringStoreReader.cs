using CollectionConfig.net.Core.Interfaces;

namespace CollectionConfig.net.Logic.StorageReaders.String;

/// <summary>
/// Reads data in from CSV file
/// </summary>
public class StringStoreReader : IDataStoreReader
{
    private readonly string _storedString;

    /// <param name="mutableString">The mutable string containing what to read</param>
    public StringStoreReader(string mutableString)
    {
        _storedString = mutableString;
    }
    
    /// <summary>
    /// Reads data
    /// </summary>
    /// <returns>All contents of the string</returns>
    public string Read()
    {
        return _storedString;
    }
}