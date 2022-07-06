using System;
using System.IO;
using CollectionConfig.net.Logic.StorageReaders.String;
using FluentAssertions;
using NUnit.Framework;
using Serilog;

namespace IntegrationTests.ReadTests.StorageReadersTests.String;

public class StringReaderTests
{
    private string _mutableString = 
        @"Name,Age,MilesRun,PetsName
David,32,3.15,Whiskey
Alyssa,26,2.12,Maxx
Robin,28,1.23,Snuggles
Dyamond,31,2.23,Trixie";
    
    // ReSharper disable once FieldCanBeMadeReadOnly.Local because if we uncomment logging below it won't be readonly
    private ILogger? _testLogger;
    private StringStoreReader? _sut;
    
    [SetUp]
    public void SetUp()
    {
        // Uncomment these if you want logging. Debugging seems potentially iffy on proxy objects, so logging may help:
        var logPath =
            Path.Join(Environment.GetFolderPath(
                Environment.SpecialFolder.Desktop), 
                "CollectionConfig.CsvEnumerationTests.net.log"); 
        
        _testLogger ??= TestLogger.GetTestLogger(logPath);

        _sut = new StringStoreReader(_mutableString);
    }

    [TearDown]
    public void TearDown()
    {
        _sut = null;
    }
    
    [Test]
    public void StringReader_WhenTestRuns_ShouldNotBeNull()
    {
        _sut.Should().NotBeNull();
    }
    
    [Test]
    public void Read_WhenCalled_ShouldBeStoredString()
    {
        var result = _sut?.Read();

        result.Should().Be(
            @"Name,Age,MilesRun,PetsName
David,32,3.15,Whiskey
Alyssa,26,2.12,Maxx
Robin,28,1.23,Snuggles
Dyamond,31,2.23,Trixie") ;
    }
}
