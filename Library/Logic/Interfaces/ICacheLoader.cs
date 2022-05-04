using System.Collections.Generic;
using CollectionConfig.net.Common.Models;

namespace CollectionConfig.net.Common.Logic.Interfaces;

/// <summary>
/// Used for loading data from a CSV or JSON file that represents a List of ICustomInterface into a cached state
/// that is halfway between the file on disk and the native List of ICustomInterface
/// </summary>
public interface ICacheLoader
{
    /// <summary>
    /// Returns data from the file on disk in the form of List of ProxiedListElement, presumably to be cached 
    /// </summary>
    /// <returns>Data from the file on disk in the form of List of ProxiedListElement</returns>
    List<FileElement> UpdateCachedDataFromFile();
}