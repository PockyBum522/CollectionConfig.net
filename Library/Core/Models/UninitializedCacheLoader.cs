using CollectionConfig.net.Core.Interfaces;

namespace CollectionConfig.net.Core.Models;

/// <summary>
/// Handles taking a CSV and making a proxy list with all the values
/// </summary>
public class UninitializedCacheLoader : ICacheLoader
{
    /// <summary>
    /// This should never be run, but could be if user doesn't call .UseCsvFile() or .UseJsonFile() on a builder
    /// </summary>
    /// <returns>Nothing!</returns>
    /// <exception cref="ArgumentException">Should always be thrown</exception>
    public List<FileElement> UpdateCachedDataFromFile()
    {
            throw new ArgumentException("Before using .Build() on a CollectionConfigurationBuilder, you MUST " +
                                        "either call .UseCsvFile() or .UseJsonFile() on the builder");
    }
}