using System;
using System.Reflection;
using Castle.DynamicProxy;
using CollectionConfig.net.Common.Core;
using CollectionConfig.net.Common.Logic.Csv;

namespace CollectionConfig.net.Common;

/// <summary>
/// Allows for building the CollectionConfiguration later
/// </summary>
public class CollectionConfigurationBuilder<T> where T : class
{
    internal readonly CollectionConfigurationInstanceData InstanceInstanceData;
    
    private readonly ProxyGenerator _generator = new ();
    private readonly IInterceptor _interceptor;

    /// <summary>
    /// Constructor to allow for building the CollectionConfiguration later
    /// </summary>
    /// <exception cref="ArgumentException">Throws if passed type is not an interface</exception>
    public CollectionConfigurationBuilder()
    {
        var typeInfo = typeof(T).GetTypeInfo();

        if (!typeInfo.IsInterface) 
            throw new ArgumentException($"{typeInfo.FullName} must be an interface", typeInfo.FullName);
        
        InstanceInstanceData = new CollectionConfigurationInstanceData();
        
        _interceptor = new InterfaceInterceptor<T>(InstanceInstanceData);
    }

    /// <summary>
    /// Creates an instance of the configuration interface
    /// </summary>
    /// <returns></returns>
    public T Build()
    {
        CheckThatFileFormatIsInitialized();
        
        var instance = _generator.CreateInterfaceProxyWithoutTarget<T>(_interceptor);
        
        return instance;
    }

    private void CheckThatFileFormatIsInitialized()
    {
        if (InstanceInstanceData.CacheLoader is UninitializedCacheLoader)
            throw new ArgumentException("Before using .Build() on a CollectionConfigurationBuilder, you MUST " +
                                        "either call .UseCsvFile() or .UseJsonFile() on the builder");
    }
}