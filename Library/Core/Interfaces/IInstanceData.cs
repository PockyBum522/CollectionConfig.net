using CollectionConfig.net.Core.Models;
using Serilog;

namespace CollectionConfig.net.Core.Interfaces;

/// <summary>
/// Stores internal data that is needed by most classes in this library
/// </summary>
public interface IInstanceData
{
    /// <summary>
    /// Injected logger, optional
    /// </summary>
    ILogger? Logger { get; set; }
    
    /// <summary>
    /// Full file path of the CollectionConfiguration CSV or JSON file on disk
    /// </summary>
    string FullFilePath { get; }
    
    /// <summary>
    /// Storage for cached copy of collection configuration file on disk
    /// </summary>
    public List<DataStoreElement> CachedConfigurationItems { get; set; }

    /// <summary>
    /// The CacheLoader implementation to use, differs between file formats
    /// </summary>
    public ICacheLoader CacheLoader { get; set; }
}