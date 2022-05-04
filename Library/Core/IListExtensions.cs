using System;
using System.Collections.Generic;
using System.ComponentModel;
using Castle.DynamicProxy;

namespace CollectionConfig.net.Common.Core;

/// <summary>
/// Sets up extension for GetBlankItem() on IList
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Reference to InstanceData for access to the InterfaceInterceptor
    /// </summary>
    public static CollectionConfigurationInstanceData? InstanceData;
    
    private static readonly ProxyGenerator Generator = new ();

    /// <summary>
    /// Extension method for IList to allow generation of a new blank Interface Proxy (An element of that IList)
    /// So that it can later be added to the IList and types will match
    /// </summary>
    /// <param name="inputList">The IList to extend</param>
    /// <typeparam name="T">The type of Elements in the IList, must be an interface</typeparam>
    /// <returns>New blank element of same type as elements in IList</returns>
    public static T GenerateNewElement<T>(this IList<T> inputList)
    {
        if (InstanceData is null) 
            throw new ArgumentNullException(nameof(InstanceData));
        
        var itemProxy = Generator.CreateInterfaceProxyWithoutTarget(typeof(T), InstanceData.Interceptor);

        var convertedItemProxy = (T)itemProxy;
        
        return convertedItemProxy;
    }
}