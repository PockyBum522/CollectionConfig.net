using System.Reflection;
using Castle.DynamicProxy;
using CollectionConfig.net.Core.Models;
using CollectionConfig.net.Logic;
using CollectionConfig.net.Logic.InterfaceInterceptors;
using JetBrains.Annotations;
using Serilog;
using Serilog.Core;

namespace CollectionConfig.net.Main;

/// <summary>
/// Allows for building the CollectionConfiguration later
///
/// All dependency injection takes place either here or in CollectionConfigurationExtensions
/// </summary>
[PublicAPI]
public class CollectionConfigurationBuilder<T> where T : class
{
    /// <summary>
    /// This will be created in the constructor for CollectionConfigurationBuilder, then will be accessed by
    /// CollectionConfigurationExtensions to set the full file path of the config file.
    ///
    /// InstanceData is then injected into most of the classes in this library. 
    /// </summary>
    internal InstanceData InstanceData;
    
    private readonly ProxyGenerator _generator = new ();
    
    /// <summary>
    /// Constructor to allow for building the CollectionConfiguration later
    /// </summary>
    /// <exception cref="ArgumentException">Throws if passed type is not an interface</exception>
    public CollectionConfigurationBuilder()
    {
        var typeInfo = typeof(T).GetTypeInfo();

        if (!typeInfo.IsInterface) 
            throw new ArgumentException($"{typeInfo.FullName} must be an interface", typeInfo.FullName);

        // Set up InstanceData, which will be accessed by CollectionConfigurationExtensions to set the full file path
        // of the config file. InstanceData is then injected into most of the classes in this library. 
        InstanceData = new InstanceData(
            "", 
            new List<DataStoreElement>(),
            new UninitializedCacheLoader());
    }

    /// <summary>
    /// Creates an instance of the configuration interface as a proxy object for the interface
    /// </summary>
    /// <returns></returns>
    public T Build()
    {
        CheckThatFileFormatIsInitialized();
        
        // Below here is all setting up and injecting dependencies
        var interceptor = new InterfaceInterceptor<T>(InstanceData);
        
        var instance = _generator.CreateInterfaceProxyWithoutTarget<T>(interceptor);
        
        return instance;
    }

    private void CheckThatFileFormatIsInitialized()
    {
        if (InstanceData.CacheLoader is UninitializedCacheLoader)
            throw new ArgumentException("Before using .Build() on a CollectionConfigurationBuilder, you MUST " +
                                        "either call .UseCsvFile() or .UseJsonFile() on the builder");
    }
}