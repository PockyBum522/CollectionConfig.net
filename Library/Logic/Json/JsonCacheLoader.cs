using System.Globalization;
using CollectionConfig.net.Core.Interfaces;
using CollectionConfig.net.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CollectionConfig.net.Logic.Json;

/// <summary>
/// Handles taking a CSV and making a proxy list with all the values
/// </summary>
public class JsonCacheLoader : ICacheLoader
{
    private readonly string _fullFilePath;
    private readonly IFileReader _fileReader;

    /// <summary>
    /// Constructor to take injected dependencies
    /// </summary>
    /// <param name="fullFilePath">Full path of the JSON file containing configuration data</param>
    /// <param name="jsonFileReader">Injected</param>
    public JsonCacheLoader(string fullFilePath, JsonFileReader jsonFileReader)
    {
        _fullFilePath = fullFilePath;
        _fileReader = jsonFileReader;
    }
    
    private FileElement MakeFileElementFrom(JToken jsonToken, int positionInList)
    {
        var returnFileElement = new FileElement()
        {
            PositionInList = positionInList
        };

        foreach (var jToken in jsonToken.Children())
        {
            var jElementToken = (JProperty)jToken;
            var elementValue = (JValue)(jElementToken.First ?? throw new ArgumentNullException());

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
    public List<FileElement> UpdateCachedDataFromFile()
    {
        var rawJsonData = _fileReader.Read(_fullFilePath);
        
        var returnList = new List<FileElement>();
        
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