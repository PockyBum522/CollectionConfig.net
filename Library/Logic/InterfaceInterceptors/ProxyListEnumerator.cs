using System.Collections;
using Castle.DynamicProxy;
using CollectionConfig.net.Core.Interfaces;
using Serilog;

namespace CollectionConfig.net.Logic.InterfaceInterceptors;

/// <summary>
/// Used for handling the intercepted GetEnumerator method on our cached items that represent the IList of
/// ICustomInterface
/// </summary>
public class ProxyListEnumerator<TIListOfCustomInterface, TNestedCustomInterface> : IEnumerator<TNestedCustomInterface> where TNestedCustomInterface : class
{
    private int _index = -1;
    
    private readonly IInstanceData _instanceData;
    private readonly ILogger? _logger = null;
    
    /// <summary>
    /// Constructor to get our dependencies that are injected
    /// </summary>
    /// <param name="instanceData">Injected</param>
    public ProxyListEnumerator(IInstanceData instanceData)
    {
        _instanceData = instanceData;
        _logger = instanceData.Logger;
    }
    
    /// <summary>
    /// Checks if we have another item to move to
    /// </summary>
    /// <returns>True if we can keep going, false if we're at the end</returns>
    public bool MoveNext()
    {
        _logger?.Information("In {ClassName}, {MethodName} called. Current index {IncrementedIndex}",
            nameof(ProxyListEnumerator<TIListOfCustomInterface, TNestedCustomInterface>), nameof(MoveNext), _index + 1);
        
        return ++_index < _instanceData.CachedConfigurationItems.Count;
    }

    /// <summary>
    /// Resets the enumerator to the beginning
    /// </summary>
    public void Reset()
    {
        _index = -1;
        
        _logger?.Information("In {ClassName}, {MethodName} called. Current index {UnIncrementedIndex}",
            nameof(ProxyListEnumerator<TIListOfCustomInterface, TNestedCustomInterface>), nameof(Reset), _index);
    }

    /// <summary>
    /// The current object the enumerator is on as type T
    /// </summary>
    public TNestedCustomInterface Current => GetCurrentObject(_index);

    private TNestedCustomInterface GetCurrentObject(int index)
    {
        _logger?.Information("In {ClassName}, {MethodName} called. Current index {UnIncrementedIndex}",
            nameof(ProxyListEnumerator<TIListOfCustomInterface, TNestedCustomInterface>), nameof(GetCurrentObject), _index);
        
        var cachedElement = _instanceData.CachedConfigurationItems[index];
        
        var interceptor = new GetNewElementInterfaceInterceptor<TNestedCustomInterface>();
        
        var itemProxy = new ProxyGenerator().CreateInterfaceProxyWithoutTarget<TNestedCustomInterface>(interceptor);

        var proxyObjectProperties = itemProxy.GetType().GetProperties();
        
        foreach (var propertyInfo in proxyObjectProperties)
        {
            
        }
        
        return itemProxy;
    }

    object IEnumerator.Current => GetCurrentObject(_index);

    /// <summary>
    /// Unused because the file reads and writes close on their own
    /// </summary>
    public void Dispose() { }
}

/// <summary>
/// Used for handling the intercepted GetEnumerator method on our cached items that represent the IList of
/// ICustomInterface
/// </summary>
public class ProxyListEnumerable<TIListOfCustomInterface> : IEnumerable<TIListOfCustomInterface> where T: class 
{
    private readonly IInstanceData _instanceData;
    
    /// <summary>
    /// Constructor to get our dependencies that are injected
    /// </summary>
    /// <param name="instanceData">Injected</param>
    public ProxyListEnumerable(IInstanceData instanceData)
    {
        _instanceData = instanceData;
    }
    
    /// <summary>
    /// Gets Enumerator for cached items as IEnumerator of FileElement
    /// </summary>
    /// <returns>Enumerator for cached items as IEnumerator of FileElement</returns>
    public IEnumerator<TIListOfCustomInterface> GetEnumerator()
    {
        return new ProxyListEnumerator<TIListOfCustomInterface>(_instanceData);
    }

    /// <summary>
    /// UNUSED: Gets Enumerator for cached items as IEnumerator
    /// </summary>
    /// <exception cref="NotImplementedException">Throws always</exception>
    /// <returns>Nothing!</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}