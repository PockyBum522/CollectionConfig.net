using CollectionConfig.net.Core.Interfaces;
using CollectionConfig.net.Core.Models;
using JetBrains.Annotations;

namespace CollectionConfig.net.Logic.CacheLoaders;

/// <summary>
/// Handles taking a CSV and making a proxy list with all the values
/// </summary>
[PublicAPI]
public class CsvCacheLoader : ICacheLoader
{
    private readonly string _fullFilePath;
    private readonly IFileReader _fileReader;
    
    private int _positionInCsv;

    /// <summary>
    /// Constructor to take injected dependencies
    /// </summary>
    /// <param name="fullFilePath">Full path to CSV file containing configuration data</param>
    /// <param name="fileReader">Injected</param>
    public CsvCacheLoader(string fullFilePath, IFileReader fileReader)
    {
        _fullFilePath = fullFilePath;
        _fileReader = fileReader;
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
        var rawCsvData = _fileReader.Read(_fullFilePath);
        
        var returnList = new List<FileElement>();

        var headers = GetHeaders();
        
        _positionInCsv = 0;
         
        foreach (var line in rawCsvData.Split(Environment.NewLine))
        {
            if (_positionInCsv == 0)
            {
                // Skip headers row
                _positionInCsv++;
                continue;
            }
            
            var instance = GetCachedElementLoadedWithCsvData(line, headers);

            returnList.Add(instance);
        
            _positionInCsv++;
        }

        return returnList;
    }
    
    /// <summary>
    /// Gets headers in configuration file in order 
    /// </summary>
    /// <returns>List of string representing the header row values, in order</returns>
    public List<string> GetHeaders()
    {
        var rawCsvData = _fileReader.Read(_fullFilePath);
        
        var headers = new List<string>();
        
        var headerLine = rawCsvData
            .Split(Environment.NewLine)
            .FirstOrDefault();

        foreach (var header in headerLine?.Split(",") ?? 
                               throw new FileLoadException("In attempt to get headers, " +
                                                           $"{nameof(headerLine)} was null"))
        {
            headers.Add(header);
        }
        
        return headers;
    }
}