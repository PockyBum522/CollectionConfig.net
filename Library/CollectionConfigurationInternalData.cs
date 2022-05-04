using System.Collections.Generic;
using CollectionConfig.net.Common.Models;

namespace CollectionConfig.net.Common;

/// <summary>
/// Stores internal data that is needed by most classes in this library
/// </summary>
public class CollectionConfigurationInternalData
{
    /// <summary>
    /// Full file path of the CollectionConfiguration CSV or JSON file on disk
    /// </summary>
    public string FullFilePath { get; set; } = "";

    /// <summary>
    /// Storage for cached copy of collection configuration file on disk
    /// </summary>
    public readonly List<FileElement> CachedConfigurationItems = new();
}