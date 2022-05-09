using CollectionConfig.net.Core.Interfaces;
using CollectionConfig.net.Core.Models;

namespace CollectionConfig.net.Logic.Csv;

/// <summary>
/// Handles taking a CSV and making a proxy list with all the values
/// </summary>
public class CsvCacheLoader : ICacheLoader
{
    private readonly string _fullFilePath;
    private readonly IFileReader _csvFileReader;
    
    private int _positionInCsv;

    /// <summary>
    /// Constructor to take injected dependencies
    /// </summary>
    /// <param name="fullFilePath">Full path to CSV file containing configuration data</param>
    /// <param name="csvFileReader">Injected</param>
    public CsvCacheLoader(string fullFilePath, CsvFileReader csvFileReader)
    {
        _fullFilePath = fullFilePath;
        _csvFileReader = csvFileReader;
    }
    
    private List<string> ReadHeaders(string line)
    {
        var splitHeaders = line.Split(",");

        return splitHeaders.ToList();
    }

    private FileElement GetCachedElementLoadedWithCsvData(string line, List<string> headers)
    {
        var proxiedListElement = new FileElement();
        
        var splitLine = line.Split(",");
        
        for (var i = 0; i < headers.Count; i++)
        {
            var headerName = headers[i];
            var lineValue = splitLine[i];

            proxiedListElement.PositionInList = _positionInCsv;
            
            proxiedListElement.StoredValues.Add(new KeyValuePair<string, string>(headerName, lineValue));    
        }
        
        return proxiedListElement;
    }

    /// <summary>
    /// Returns data from the CSV on disk in the form of List of ProxiedListElement, presumably to be cached 
    /// </summary>
    /// <returns>Data from the CSV on disk in the form of List of ProxiedListElement</returns>
    public List<FileElement> UpdateCachedDataFromFile()
    {
        var rawCsvData = _csvFileReader.Read(_fullFilePath);
        
        var returnList = new List<FileElement>();

        var headers = new List<string>();
        
        _positionInCsv = 0;
         
        foreach (var line in rawCsvData.Split(Environment.NewLine))
        {
            if (_positionInCsv == 0)
            {
                headers = ReadHeaders(line);
                _positionInCsv++;
                continue;
            }
            
            var instance = GetCachedElementLoadedWithCsvData(line, headers);

            returnList.Add(instance);
        
            _positionInCsv++;
        }

        return returnList;
    }
}