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
    private readonly IDataStoreReader _dataStoreReader;
    
    private int _positionInCsv;

    /// <summary>
    /// Constructor to take injected dependencies
    /// </summary>
    /// <param name="dataStoreReader">Injected</param>
    public CsvCacheLoader(IDataStoreReader dataStoreReader)
    {
        _dataStoreReader = dataStoreReader;
    }
    
    private DataStoreElement GetCachedElementLoadedWithCsvData(string line, List<string> headers)
    {
        var proxiedListElement = new DataStoreElement();
        
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
    /// Returns data from the CSV on disk in the form of List of FileElement, presumably to be cached 
    /// </summary>
    /// <returns>Data from the CSV on disk in the form of List of FileElement</returns>
    public List<DataStoreElement> UpdateCachedDataFromFile()
    {
        var rawCsvData = _dataStoreReader.Read();
        
        var returnList = new List<DataStoreElement>();

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
        var rawCsvData = _dataStoreReader.Read();
        
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