namespace CollectionConfig.net.UnitTests.TestResources;

public class RawData
{
    public static string CsvRaw =>
        @"Name,Age,MilesRun,PetsName
David,32,3.15,Whiskey
Alyssa,26,2.12,Maxx
Robin,28,1.23,Snuggles
Dyamond,31,2.23,Trixie";
    
    public static string JsonRaw =>
    @"[
        {
            ""Name"": ""David"",
            ""Age"": 32,
            ""MilesRun"": 3.15,
            ""PetsName"": ""Whiskey""
        },
        {
            ""Name"": ""Alyssa"",
            ""Age"": 26,
            ""MilesRun"": 2.12,
            ""PetsName"": ""Maxx""
        },
        {
            ""Name"": ""Robin"",
            ""Age"": 28,
            ""MilesRun"": 1.23,
            ""PetsName"": ""Snuggles""
        },
        {
            ""Name"": ""Dyamond"",
            ""Age"": 31,
            ""MilesRun"": 2.23,
            ""PetsName"": ""Trixie""
        }
    ]";
}