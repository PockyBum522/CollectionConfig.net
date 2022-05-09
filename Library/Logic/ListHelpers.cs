using Castle.DynamicProxy;

namespace CollectionConfig.net.Logic;

/// <summary>
/// Sets up extension for GetBlankItem() on IList
/// </summary>
public class ListHelpers
{
    private readonly IInterceptor _interceptor;
    private readonly ProxyGenerator Generator = new ();

    /// <summary>
    /// Constructor to set up injected dependencies
    /// </summary>
    /// <param name="interceptor">Injected</param>
    public ListHelpers(IInterceptor interceptor)
    {
        _interceptor = interceptor;
    }
    
    /// <summary>
    /// Extension method for IList to allow generation of a new blank Interface Proxy (An element of that IList)
    /// So that it can later be added to the IList and types will match
    /// </summary>
    /// <param name="inputList">The IList to extend</param>
    /// <typeparam name="T">The type of Elements in the IList, must be an interface</typeparam>
    /// <returns>New blank element of same type as elements in IList</returns>
    public T GenerateNewElement<T>(IList<T> inputList)
    {
        var itemProxy = Generator.CreateInterfaceProxyWithoutTarget(typeof(T), _interceptor);

        var convertedItemProxy = (T)itemProxy;
        
        return convertedItemProxy;
    }
}