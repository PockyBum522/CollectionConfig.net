using CollectionConfig.net.Logic;
using CollectionConfig.net.Logic.CacheLoaders;

namespace CollectionConfig.net.Main;

/// <summary>
/// Configuration extensions
/// </summary>
public static class CollectionConfigurationExtensions
{
   /// <summary>
   /// Simple CSV storage.
   /// </summary>
   /// <param name="builder">The builder to apply using a CSV file to</param>
   /// <param name="csvFilePath">File does not have to exist, however it will be created as soon as you try to write to it.</param>
   /// <returns></returns>
   public static CollectionConfigurationBuilder<TInterface> UseCsvFile<TInterface>(
      this CollectionConfigurationBuilder<TInterface> builder,
      string csvFilePath) where TInterface : class
   {
      // Add CSV specific dependencies
      builder.InstanceData.FullFilePath = csvFilePath;
      builder.InstanceData.CacheLoader = new CsvCacheLoader(csvFilePath, new FileReader());
      
      return builder;
   }
   
   /// <summary>
   /// Simple JSON storage.
   /// </summary>
   /// <param name="builder">The builder to apply using a JSON file to</param>
   /// <param name="jsonFilePath">File does not have to exist, however it will be created as soon as you try to write to it.</param>
   /// <returns></returns>
   public static CollectionConfigurationBuilder<TInterface> UseJsonFile<TInterface>(
      this CollectionConfigurationBuilder<TInterface> builder,
      string jsonFilePath) where TInterface : class
   {
      // Add JSON specific dependencies
      builder.InstanceData.FullFilePath = jsonFilePath;
      builder.InstanceData.CacheLoader = new JsonCacheLoader(jsonFilePath, new FileReader());
      
      return builder;
   }
}