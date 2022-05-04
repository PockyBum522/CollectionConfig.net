namespace CollectionConfig.net.Common;

/// <summary>
/// Configuration extensions
/// </summary>
public static class CollectionConfigurationExtensions
{
   // TODO: Add this functionality
   
   /// <summary>
   /// Simple CSV storage.
   /// </summary>
   /// <param name="builder"></param>
   /// <param name="csvFilePath">File does not have to exist, however it will be created as soon as you try to write to it.</param>
   /// <returns></returns>
   public static CollectionConfigurationBuilder<TInterface> UseCsvFile<TInterface>(
      this CollectionConfigurationBuilder<TInterface> builder,
      string csvFilePath) where TInterface : class
   {
      builder.InstanceInternalData.FullFilePath = csvFilePath;
      
      return builder;
   }
}