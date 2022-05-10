using Castle.DynamicProxy;
using CollectionConfig.net.Logic.InterfaceInterceptors;
using JetBrains.Annotations;

namespace CollectionConfig.net.Main;

/// <summary>
/// Sets up extension for GetBlankItem() on IList
/// </summary>
[PublicAPI]
public static class ListExtensions
{
    /// <summary>
    /// Extension method for IList to allow generation of a new blank Interface Proxy (An element of that IList)
    /// So that it can later be added to the IList and types will match
    /// </summary>
    /// <param name="inputList">The IList to extend</param>
    /// <typeparam name="T">The type of Elements in the IList, must be an interface</typeparam>
    /// <returns>New blank element of same type as elements in IList</returns>
    public static T GetNewElement<T>(this IList<T> inputList) where T : class
    {
        var interceptor = new GetNewElementInterfaceInterceptor<T>();

        var itemProxy = new ProxyGenerator().CreateInterfaceProxyWithoutTarget(typeof(T), interceptor);

        var convertedItemProxy = (T)itemProxy;
        
        return convertedItemProxy;
    }
}