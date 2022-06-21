using System;
using System.Collections.Generic;
using System.IO;
using CollectionConfig.net.Core.Interfaces;
using CollectionConfig.net.Core.Models;
using CollectionConfig.net.Logic.CacheLoaders;
using CollectionConfig.net.Main;
using CollectionConfig.net.UnitTests.Interfaces;
using CollectionConfig.net.UnitTests.TestResources;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Serilog;

namespace CollectionConfig.net.UnitTests.ReadTests.CSV;

public class CsvCacheLoaderTests
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Local because if we uncomment logging below it won't be readonly
    private ILogger? _testLogger = null;
    private readonly IFileReader _fileReaderMock = Substitute.For<IFileReader>();
    private CsvCacheLoader? _sut = null;
    
    [SetUp]
    public void SetUp()
    {
        // Uncomment these if you want logging. Debugging seems potentially iffy on proxy objects, so logging may help:
        var logPath =
            Path.Join(Environment.GetFolderPath(
                Environment.SpecialFolder.Desktop), 
                "CollectionConfig.CsvEnumerationTests.net.log"); 
        
        _testLogger ??= TestLogger.GetTestLogger(logPath);

        _sut = new CsvCacheLoader("", _fileReaderMock);
        
        _fileReaderMock.Read("").Returns(RawData.CsvRaw);
    }

    [TearDown]
    public void TearDown()
    {
        _sut = null;
    }
    
    [Test]
    public void ThisTest_WhenStarted_SutShouldNotBeNull()
    {
        var result = _sut;

        result.Should().NotBeNull();
    }
    
    [Test]
    public void GetHeaders_WhenCalled_ShouldGetCorrectHeaders()
    {
        var result = _sut?.GetHeaders();

        result.Should().ContainInOrder("Name", "Age", "MilesRun", "PetsName");
    }    
    
    [Test]
    public void UpdateCachedDataFromFile_WhenCalled_ShouldGetCorrectFirstFileElementName()
    {
        var result = _sut?.UpdateCachedDataFromFile()[0];

        result?.PositionInList.Should().Be(1);
        
        result?.StoredValues[0].Should().Be(new KeyValuePair<string, string>("Name", "David"));
    }

    [Test]
    public void UpdateCachedDataFromFile_WhenCalled_ShouldGetCorrectLastFileElementName()
    {
        var result = _sut?.UpdateCachedDataFromFile()[3];

        result?.PositionInList.Should().Be(4);
        
        result?.StoredValues[0].Should().Be(new KeyValuePair<string, string>("Name", "Dyamond"));
    }
    
    [Test]
    public void UpdateCachedDataFromFile_WhenCalled_ShouldGetCorrectFirstFileElementPetsName()
    {
        var result = _sut?.UpdateCachedDataFromFile()[0];

        result?.PositionInList.Should().Be(1);
        
        result?.StoredValues[3].Should().Be(new KeyValuePair<string, string>("PetsName", "Whiskey"));
    }

    [Test]
    public void UpdateCachedDataFromFile_WhenCalled_ShouldGetCorrectLastFileElementPetsName()
    {
        var result = _sut?.UpdateCachedDataFromFile()[3];

        result?.PositionInList.Should().Be(4);
        
        result?.StoredValues[3].Should().Be(new KeyValuePair<string, string>("PetsName", "Trixie"));
    }
    
    [Test]
    public void UpdateCachedDataFromFile_WhenCalled_ShouldGetCorrectFirstFileElementAge()
    {
        var result = _sut?.UpdateCachedDataFromFile()[0];

        result?.PositionInList.Should().Be(1);
        
        result?.StoredValues[1].Should().Be(new KeyValuePair<string, string>("Age", "32"));
    }

    [Test]
    public void UpdateCachedDataFromFile_WhenCalled_ShouldGetCorrectLastFileElementAge()
    {
        var result = _sut?.UpdateCachedDataFromFile()[3];

        result?.PositionInList.Should().Be(4);

        result?.StoredValues[1].Should().Be(new KeyValuePair<string, string>("Age", "31"));
    }
    
    [Test]
    public void UpdateCachedDataFromFile_WhenCalled_ShouldGetCorrectFirstFileElementMilesRun()
    {
        var result = _sut?.UpdateCachedDataFromFile()[0];

        result?.PositionInList.Should().Be(1);
        
        result?.StoredValues[2].Should().Be(new KeyValuePair<string, string>("MilesRun", "3.15"));
    }

    [Test]
    public void UpdateCachedDataFromFile_WhenCalled_ShouldGetCorrectLastFileElementMilesRun()
    {
        var result = _sut?.UpdateCachedDataFromFile()[3];

        result?.PositionInList.Should().Be(4);

        result?.StoredValues[2].Should().Be(new KeyValuePair<string, string>("MilesRun", "2.23"));
    }
}
