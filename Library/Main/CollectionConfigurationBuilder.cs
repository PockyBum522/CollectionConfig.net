using System.Reflection;
using Castle.DynamicProxy;
using CollectionConfig.net.Core.Models;
using CollectionConfig.net.Logic;
using JetBrains.Annotations;

namespace CollectionConfig.net.Main;

/// <summary>
/// Allows for building the CollectionConfiguration later
/// </summary>
[PublicAPI]
public class CollectionConfigurationBuilder<T> where T : class
{
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

        InstanceData = new InstanceData(
            "", 
            new List<FileElement>(),
            new UninitializedCacheLoader());
    }

    /// <summary>
    /// Creates an instance of the configuration interface as a proxy object for the interface
    /// </summary>
    /// <returns></returns>
    public T Build()
    {
        var interceptor = new InterfaceInterceptor<T>(InstanceData);
        
        CheckThatFileFormatIsInitialized();
        
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