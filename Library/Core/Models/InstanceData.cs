using CollectionConfig.net.Core.Interfaces;
using Serilog;

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
        List<DataStoreElement> cachedConfigurationItems, 
        UninitializedCacheLoader cacheLoader)
    {
        FullFilePath = fullFilePath;
        CachedConfigurationItems = cachedConfigurationItems;
        CacheLoader = cacheLoader;
    }

    /// <summary>
    /// Gets set as true once DataStoreReader, DataStoreWriter, and CacheLoader are set up, so that user doesn't
    /// accidentally call initialization methods that would clash
    /// </summary>
    public bool IsInitialized { get; set; } = false;
    
    /// <summary>
    /// Injected logger, optional
    /// </summary>
    public ILogger? Logger { get; set; } = null;

    /// <summary>
    /// Full file path of the CollectionConfiguration CSV or JSON file on disk
    /// </summary>
    public string FullFilePath { get; set; }

    /// <summary>
    /// Storage for cached copy of collection configuration file on disk
    /// </summary>
    public List<DataStoreElement> CachedConfigurationItems { get; set; }

    /// <summary>
    /// The CacheLoader implementation to use, differs between file formats
    /// </summary>
    public ICacheLoader CacheLoader { get; set; }

    /// <summary>
    /// Data store writer that formats new elements for the particular format of the storage and handles
    /// writing those formatted elements to storage
    /// </summary>
    public IDataStoreWriter? DataStoreWriter { get; set; }
    
    /// <summary>
    /// Data store reader that gets new elements from the storage
    /// </summary>
    public IDataStoreReader? DataStoreReader { get; set; }
}