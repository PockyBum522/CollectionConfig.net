using Castle.DynamicProxy;
using JetBrains.Annotations;

namespace CollectionConfig.net.Main;

/// <summary>
/// Sets up extension for GetBlankItem() on IList
/// </summary>
[PublicAPI]
public static class ListExtensions
{
    /// <summary>
    /// Property for injection of IInterceptor
    /// </summary>
    public static IInterceptor? Interceptor;
    private static ProxyGenerator _generator = new ();
    
    /// <summary>
    /// Extension method for IList to allow generation of a new blank Interface Proxy (An element of that IList)
    /// So that it can later be added to the IList and types will match
    /// </summary>
    /// <param name="inputList">The IList to extend</param>
    /// <typeparam name="T">The type of Elements in the IList, must be an interface</typeparam>
    /// <returns>New blank element of same type as elements in IList</returns>
    public static T GenerateNewElement<T>(this IList<T> inputList)
    {
        if (Interceptor is null) throw new Exception($"{nameof(Interceptor)} was null in GenerateNewElement" +
                                                     $"this means {nameof(Interceptor)} was not properly injected");

        var itemProxy = _generator.CreateInterfaceProxyWithoutTarget(typeof(T), Interceptor);

        var convertedItemProxy = (T)itemProxy;
        
        return convertedItemProxy;
    }
}