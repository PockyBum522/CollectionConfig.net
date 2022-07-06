namespace CollectionConfig.net.Core.Interfaces;

/// <summary>
/// Reads data from a specified CSV file
/// </summary>
public interface IDataStoreReader
{
    /// <summary>
    /// Reads data in from file
    /// </summary>
    /// <returns>All contents of the file as one string</returns>
    string Read();
}