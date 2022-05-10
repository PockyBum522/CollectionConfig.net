using CollectionConfig.net.Logic;
using CollectionConfig.net.Logic.CacheLoaders;
using CollectionConfig.net.Logic.Writers;
using Serilog;

namespace CollectionConfig.net.Main;

/// <summary>
/// Configuration extensions
/// </summary>
public static class CollectionConfigurationExtensions
{
   /// <summary>
   /// Optional extension method to allow for injecting a logger. If no logger is injected, then all logging is disabled
   /// </summary>
   /// <param name="builder">The builder to inject the logger into</param>
   /// <param name="logger">ILogger to inject</param>
   /// <typeparam name="TInterface">Type parameter of the IList of ICustomInterface</typeparam>
   /// <returns>Builder with logger injected</returns>
   public static CollectionConfigurationBuilder<TInterface> UseLogger<TInterface>(
      this CollectionConfigurationBuilder<TInterface> builder,
      ILogger logger) where TInterface : class
   {
      // Inject logger into InstanceData
      builder.InstanceData.Logger = logger;
      
      return builder;
   }
   
   /// <summary>
   /// Simple CSV storage.
   /// </summary>
   /// <param name="builder">The builder to apply using a CSV file to</param>
   /// <param name="csvFilePath">File does not have to exist, however it will be created as soon as you try to write to it.</param>
   /// <typeparam name="TInterface">Type parameter of the IList of ICustomInterface</typeparam>
   /// <returns>Builder with CSV specific dependencies set up and injected</returns>
   public static CollectionConfigurationBuilder<TInterface> UseCsvFile<TInterface>(
      this CollectionConfigurationBuilder<TInterface> builder,
      string csvFilePath) where TInterface : class
   {
      // Add CSV specific dependencies
      builder.InstanceData.FullFilePath = csvFilePath;
      builder.InstanceData.CacheLoader = new CsvCacheLoader(csvFilePath, new FileReader());
      builder.InstanceData.FileWriter = new CsvFileWriter(csvFilePath);
      
      return builder;
   }
   
   /// <summary>
   /// Simple JSON storage.
   /// </summary>
   /// <param name="builder">The builder to apply using a JSON file to</param>
   /// <param name="jsonFilePath">File does not have to exist, however it will be created as soon as you try to write to it.</param>
   /// <typeparam name="TInterface">Type parameter of the IList of ICustomInterface</typeparam>
   /// <returns>Builder with JSON specific dependencies set up and injected</returns>
   public static CollectionConfigurationBuilder<TInterface> UseJsonFile<TInterface>(
      this CollectionConfigurationBuilder<TInterface> builder,
      string jsonFilePath) where TInterface : class
   {
      // Add JSON specific dependencies
      builder.InstanceData.FullFilePath = jsonFilePath;
      builder.InstanceData.CacheLoader = new JsonCacheLoader(jsonFilePath, new FileReader());
      builder.InstanceData.FileWriter = new JsonFileWriter(jsonFilePath);
      
      return builder;
   }
}