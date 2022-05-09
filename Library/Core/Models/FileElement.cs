using JetBrains.Annotations;

namespace CollectionConfig.net.Core.Models;

/// <summary>
/// Stores the data for a list element that will be proxied from an Interface along with its
/// position in the proxied list
/// </summary>
[PublicAPI]
public class FileElement
{
    /// <summary>
    /// Key value pairs to store the value for each proxied property name (Keys are property names)
    /// </summary>
    public List<KeyValuePair<string, string>> StoredValues { get; set; } = new();
    
    /// <summary>
    /// Storage for the position in the proxied list
    /// </summary>
    /// <exception cref="ArgumentException">Throws if value passed is a negative number</exception>
    public int PositionInList
    {
        get => _positionInList;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Cannot have position in list be less than 0");
            }

            _positionInList = value;
        }
    }

    /// <summary>
    /// Initialized when ProxiedListElement is created, represents how long since data was read from disk
    /// </summary>
    public DateTime LastUpdatedFromDisk { get; } 
    
    // Private
    private int _positionInList;

    /// <summary>
    /// Constructor for ProxiedListElement, initializes LastUpdatedFromDisk
    /// </summary>
    public FileElement()
    {
        LastUpdatedFromDisk = DateTime.Now;
    }
}