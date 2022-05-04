using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using CollectionConfig.net.Common.Logic.Interfaces;
using CollectionConfig.net.Common.Models;

namespace CollectionConfig.net.Common.Logic.Csv;

/// <summary>
/// Handles taking a CSV and making a proxy list with all the values
/// </summary>
public class CsvCacheLoader : ICacheLoader
{
    private readonly ProxyGenerator _generator = new ();
    private readonly IFileReader _csvFileReader;
    
    private int _positionInCsv;
        
    /// <summary>
    /// Constructor to take injected dependencies
    /// </summary>
    /// <param name="csvFileReader">Injected</param>
    public CsvCacheLoader(CsvFileReader csvFileReader)
    {
        _csvFileReader = csvFileReader;
    }

    /// <summary>
    /// Returns data from the CSV on disk in the form of List of ProxiedListElement, presumably to be cached 
    /// </summary>
    /// <param name="instanceData">Injected so that we have the FilePath</param>
    /// <returns>Data from the CSV on disk in the form of List of ProxiedListElement</returns>
    public List<FileElement> UpdateCachedDataFromFile(CollectionConfigurationInstanceData instanceData)
    {
        var rawCsvData = _csvFileReader.Read(instanceData.FullFilePath);
        
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
}