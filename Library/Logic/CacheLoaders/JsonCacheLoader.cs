using System.Globalization;
using CollectionConfig.net.Core.Interfaces;
using CollectionConfig.net.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CollectionConfig.net.Logic.CacheLoaders;

/// <summary>
/// Handles taking a CSV and making a proxy list with all the values
/// </summary>
public class JsonCacheLoader : ICacheLoader
{
    private readonly IDataStoreReader _dataStoreReader;

    /// <summary>
    /// Constructor to take injected dependencies
    /// </summary>
    /// <param name="dataStoreReader">Injected</param>
    public JsonCacheLoader(IDataStoreReader dataStoreReader)
    {
        _dataStoreReader = dataStoreReader;
    }
    
    private DataStoreElement MakeFileElementFrom(JToken jsonToken, int positionInList)
    {
        var returnFileElement = new DataStoreElement()
        {
            PositionInList = positionInList
        };

        foreach (var jToken in jsonToken.Children())
        {
            var jElementToken = (JProperty)jToken;
            var elementValue = (JValue)(jElementToken.First ?? throw new NullReferenceException());

            returnFileElement.StoredValues.Add(
                new KeyValuePair<string, string>(
                    jElementToken.Name, elementValue.ToString(CultureInfo.InvariantCulture)
                ));
        }
        
        return returnFileElement;
    }

    /// <summary>
    /// Returns data from the JSON on disk in the form of List of ProxiedListElement, presumably to be cached 
    /// </summary>
    /// <returns>Data from the JSON on disk in the form of List of ProxiedListElement</returns>
    public List<DataStoreElement> UpdateCachedDataFromFile()
    {
        var rawJsonData = _dataStoreReader.Read();
        
        var returnList = new List<DataStoreElement>();
        
        var dynamicJsonObject = JsonConvert.DeserializeObject(rawJsonData);

        var jArrayObject = (JArray)(dynamicJsonObject ?? throw new ArgumentNullException(
            nameof(dynamicJsonObject), 
            "Could not deserialize JSON data into JArray"));

        var positionInList = 0;
        
        foreach (var jsonToken in jArrayObject.Children())
        {
            var fileElement = MakeFileElementFrom(jsonToken, positionInList);
            
            returnList.Add(fileElement);

            positionInList++;
        }
        
        return returnList;
    }
}