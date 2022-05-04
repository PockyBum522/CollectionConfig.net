namespace CollectionConfig.net.Common.Logic.Csv.Interfaces;

/// <summary>
/// Reads data from a specified CSV file
/// </summary>
public interface ICsvReader
{
    /// <summary>
    /// Reads data in from csv file
    /// </summary>
    /// <param name="fullPathToCsv">Full path to the CSV file to read</param>
    /// <returns>All contents of the CSV file as one string</returns>
    string ReadCsv(string fullPathToCsv);
}