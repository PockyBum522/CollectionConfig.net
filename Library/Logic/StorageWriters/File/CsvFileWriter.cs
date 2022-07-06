using CollectionConfig.net.Core.Interfaces;
using CollectionConfig.net.Logic.CacheLoaders;
using CollectionConfig.net.Logic.StorageReaders.File;

namespace CollectionConfig.net.Logic.StorageWriters.File;

/// <summary>
/// Used to format CSV data and write that data to a configuration file or initialize new configuration file if necessary
/// </summary>
public class CsvFileWriter : IDataStoreWriter
{
    private readonly string _configurationFilePath;

    /// <summary>
    /// Constructor for injecting dependencies
    /// </summary>
    /// <param name="configurationFilePath">Full path to the configuration file, or where it will be</param>
    public CsvFileWriter(string configurationFilePath)
    {
        _configurationFilePath = configurationFilePath;
    }

    /// <summary>
    /// Gets a new element of List of ICustomInterface and formats it how it will be saved in the configuration file
    /// that represents the List of ICustomInterface
    /// </summary>
    /// <returns>Formatted string identical to how a new element looks in the configuration file that represents the
    /// List of ICustomInterface</returns>
    public string FormatNewElement(object elementToFormatForFile)
    {
        List<string> headers;
        
        if (System.IO.File.Exists(_configurationFilePath))
        {
            headers = GetExistingHeaders();
        }
        else
        {
            // TODO: If file doesn't exist, initialize it with headers from element
            throw new NotImplementedException();
        }
        
        var valuesOrderedByHeaderOrder = new string[headers.Count];
        
        // Format properties in element by order of headers
        foreach (var header in headers)
        {
            // Get position of equivalent name in headers
            var position = headers.IndexOf(header);

            var currentHeaderEquivalentProxyObjectProperty = elementToFormatForFile.GetType().GetProperty(header);
            
            if (currentHeaderEquivalentProxyObjectProperty is null)
                throw new NullReferenceException($"{nameof(currentHeaderEquivalentProxyObjectProperty)} was null");
            
            var currentPropertyValue = currentHeaderEquivalentProxyObjectProperty.GetValue(elementToFormatForFile);

            if (currentPropertyValue is null)
                throw new NullReferenceException($"{nameof(currentPropertyValue)} was null");
            
            // Add value passed in in element to appropriate position relative to header position
            valuesOrderedByHeaderOrder[position] = currentPropertyValue.ToString() ?? 
                throw new NullReferenceException($"{nameof(currentPropertyValue)} .ToString() was null");;
        }

        // Now that they're in order, we can format them for CSV
        return string.Join(",", valuesOrderedByHeaderOrder);
    }

    private List<string> GetExistingHeaders()
    {
        var csvCacheLoader = new CsvCacheLoader(new FileStoreReader(_configurationFilePath));

        return csvCacheLoader.GetHeaders();
    }

    /// <summary>
    /// Handles actually saving a new element to the configuration file that represents the List of ICustomInterface
    /// </summary>
    /// <param name="formattedNewElement">Already formatted new element</param>
    public void WriteNewElement(string formattedNewElement)
    {
        // TODO: If file exists, append text
        
        // TODO: Otherwise, initialize file

        if (System.IO.File.Exists(_configurationFilePath))
        {
            System.IO.File.AppendAllText(
                _configurationFilePath, 
                Environment.NewLine + formattedNewElement);
            
            return;
        } 
        
        // If file doesn't exist:
        throw new NotImplementedException("File not found, this means we need to initialize headers" +
                                          " but no code for that exists yet");
    }
}