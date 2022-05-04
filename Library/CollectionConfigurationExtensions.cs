using CollectionConfig.net.Common.Logic.Csv;
using CollectionConfig.net.Common.Logic.Json;

namespace CollectionConfig.net.Common;

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
      builder.BuilderInstanceData.FullFilePath = csvFilePath;
      
      builder.BuilderInstanceData.CacheLoader = new CsvCacheLoader(new CsvFileReader());
      
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
      builder.BuilderInstanceData.FullFilePath = jsonFilePath;

      builder.BuilderInstanceData.CacheLoader = new JsonCacheLoader(new FileReader());
      
      return builder;
   }
}