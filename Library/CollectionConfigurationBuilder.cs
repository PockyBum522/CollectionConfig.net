using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;
using CollectionConfig.net.Common.Core;
using CollectionConfig.net.Common.Logic.Csv;
using CollectionConfig.net.Common.Models;

namespace CollectionConfig.net.Common;

/// <summary>
/// Allows for building the CollectionConfiguration later
/// </summary>
public class CollectionConfigurationBuilder<T> where T : class
{
    internal readonly CollectionConfigurationInternalData InstanceInternalData;
    
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
        
        InstanceInternalData = new CollectionConfigurationInternalData();
        
        var csvCacheLoader = new CsvCacheLoader(new FileFileReader());
        _interceptor = new InterfaceInterceptor<T>(InstanceInternalData, csvCacheLoader);
    }

    /// <summary>
    /// Creates an instance of the configuration interface
    /// </summary>
    /// <returns></returns>
    public T Build()
    {
        T instance = _generator.CreateInterfaceProxyWithoutTarget<T>(_interceptor);
        
        return instance;
    }
}