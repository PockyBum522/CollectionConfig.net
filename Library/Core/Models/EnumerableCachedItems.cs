using System.Collections;
using CollectionConfig.net.Core.Interfaces;

namespace CollectionConfig.net.Core.Models;

/// <summary>
/// Used for handling the intercepted GetEnumerator method on our cached items that represent the IList of
/// ICustomInterface
/// </summary>
public class EnumerableCachedItems<T> : IEnumerable<T>, IEnumerator<T>
{
    private readonly IInstanceData _instanceData;
    private int _index = -1;
    
    /// <summary>
    /// Constructor to get our dependencies that are injected
    /// </summary>
    /// <param name="instanceData">Injected</param>
    public EnumerableCachedItems(IInstanceData instanceData)
    {
        _instanceData = instanceData;
    }
    
    /// <summary>
    /// Gets Enumerator for cached items as IEnumerator of FileElement
    /// </summary>
    /// <returns>Enumerator for cached items as IEnumerator of FileElement</returns>
    public IEnumerator<T> GetEnumerator()
    {
        
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

    /// <summary>
    /// Checks if we have another item to move to
    /// </summary>
    /// <returns>True if we can keep going, false if we're at the end</returns>
    public bool MoveNext()
    {
        return ++_index < _instanceData.CachedConfigurationItems.Count;
    }

    /// <summary>
    /// Resets the enumerator to the beginning
    /// </summary>
    public void Reset()
    {
        _index = -1;
    }

    /// <summary>
    /// The current object the enumerator is on as type T
    /// </summary>
    public T Current => GenerateNewElement()

    object IEnumerator.Current => Current ?? throw new InvalidCastException("Could not convert");

    /// <summary>
    /// Unused because the file reads and writes close on their own
    /// </summary>
    public void Dispose()
    {
        
    }
}