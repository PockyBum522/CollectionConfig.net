using CollectionConfig.net.Core.Interfaces;
using CollectionConfig.net.Logic;
using CollectionConfig.net.Logic.CacheLoaders;
using CollectionConfig.net.Logic.StorageReaders;
using CollectionConfig.net.Logic.StorageReaders.File;
using CollectionConfig.net.Logic.StorageWriters;
using CollectionConfig.net.Logic.StorageWriters.File;
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
      if (builder.InstanceData.IsInitialized)
         throw new InvalidOperationException("Multiple initialization methods have been called " +
                                             "on the collection configuration builder");
      
      // Add CSV specific dependencies
      builder.InstanceData.FullFilePath = csvFilePath;
      builder.InstanceData.CacheLoader = new CsvCacheLoader(new FileStoreReader(csvFilePath));
      builder.InstanceData.DataStoreWriter = new CsvFileWriter(csvFilePath);
      builder.InstanceData.DataStoreReader = new FileStoreReader(csvFilePath);
      
      builder.InstanceData.IsInitialized = true;
      
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
      if (builder.InstanceData.IsInitialized)
         throw new InvalidOperationException("Multiple initialization methods have been called " +
                                             "on the collection configuration builder");
      // Add JSON specific dependencies
      builder.InstanceData.FullFilePath = jsonFilePath;
      builder.InstanceData.CacheLoader = new JsonCacheLoader(new FileStoreReader(jsonFilePath));
      builder.InstanceData.DataStoreWriter = new JsonFileWriter(jsonFilePath);
      builder.InstanceData.DataStoreReader = new FileStoreReader(jsonFilePath);
      
      builder.InstanceData.IsInitialized = true;
      
      return builder;
   }

   /// <summary>
   /// Simple JSON storage.
   /// </summary>
   /// <param name="builder">The builder to apply using a JSON file to</param>
   /// <param name="dataStoreReader">Data storage reader to use</param>
   /// <param name="dataStoreWriter">Data storage writer to use</param>
   /// <param name="cacheLoader">CacheLoader that converts the raw storage data to List of DataStoreElement</param>
   /// <typeparam name="TInterface">Type parameter of the IList of ICustomInterface</typeparam>
   /// <returns>Builder with JSON specific dependencies set up and injected</returns>
   public static CollectionConfigurationBuilder<TInterface> UseCustomStorage<TInterface>(
      this CollectionConfigurationBuilder<TInterface> builder,
      IDataStoreReader dataStoreReader,
      IDataStoreWriter dataStoreWriter,
      ICacheLoader cacheLoader) where TInterface : class
   {
      if (builder.InstanceData.IsInitialized)
         throw new InvalidOperationException("Multiple initialization methods have been called " +
                                             "on the collection configuration builder");
      
      builder.InstanceData.DataStoreReader = dataStoreReader;
      builder.InstanceData.DataStoreWriter = dataStoreWriter;
      builder.InstanceData.CacheLoader = cacheLoader;
      
      builder.InstanceData.IsInitialized = true;
      
      return builder;
   }
}