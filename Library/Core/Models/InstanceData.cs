using CollectionConfig.net.Core.Interfaces;

namespace CollectionConfig.net.Core.Models;

/// <summary>
/// Stores internal data that is needed by most classes in this library
/// </summary>
public class InstanceData : IInstanceData
{
    /// <summary>
    /// Constructor to set up injected dependencies
    /// </summary>
    public InstanceData(
        string fullFilePath, 
        List<FileElement> cachedConfigurationItems, 
        UninitializedCacheLoader cacheLoader)
    {
        FullFilePath = fullFilePath;
        CachedConfigurationItems = cachedConfigurationItems;
        CacheLoader = cacheLoader;
    }

    /// <summary>
    /// Full file path of the CollectionConfiguration CSV or JSON file on disk
    /// </summary>
    public string FullFilePath { get; set; }

    /// <summary>
    /// Storage for cached copy of collection configuration file on disk
    /// </summary>
    public List<FileElement> CachedConfigurationItems { get; set; }

    /// <summary>
    /// The CacheLoader implementation to use, differs between file formats
    /// </summary>
    public ICacheLoader CacheLoader { get; set; }

    /// <summary>
    /// IFileWriter that formats new elements for the particular file format of the configuration file and handles
    /// writing those formatted elements to the file
    /// </summary>
    public IFileWriter? FileWriter { get; set; }
}