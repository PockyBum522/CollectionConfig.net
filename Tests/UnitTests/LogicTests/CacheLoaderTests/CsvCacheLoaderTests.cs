using System;
using System.IO;
using CollectionConfig.net.Core.Interfaces;
using CollectionConfig.net.Logic.CacheLoaders;
using CollectionConfig.net.UnitTests.TestResources;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Serilog;

namespace CollectionConfig.net.UnitTests.LogicTests.CacheLoaderTests;

public class CsvCacheLoaderTests
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Local because if we uncomment logging below it won't be readonly
    private ILogger? _testLogger;
    private CsvCacheLoader? _sut;
    private readonly IDataStoreReader _dataStoreReaderMock = Substitute.For<IDataStoreReader>();
    
    [SetUp]
    public void SetUp()
    {
        // Uncomment these if you want logging. Debugging seems potentially iffy on proxy objects, so logging may help:
        var logPath =
            Path.Join(Environment.GetFolderPath(
                Environment.SpecialFolder.Desktop), 
                "CollectionConfig.CsvEnumerationTests.net.log"); 
        
        _testLogger ??= TestLogger.GetTestLogger(logPath);

        _sut = new CsvCacheLoader(_dataStoreReaderMock);
        
        _dataStoreReaderMock.Read().Returns(RawData.CsvRaw);
    }

    [TearDown]
    public void TearDown()
    {
        _sut = null;
    }
    
    [Test]
    public void CsvCacheLoader_WhenBuiltWithMockIDataStoreReader_ShouldNotBeNull()
    {
        _sut.Should().NotBeNull();
    }
    
    [Test]
    [TestCase(0, "Name")]
    [TestCase(1, "Age")]
    [TestCase(2, "MilesRun")]
    [TestCase(3, "PetsName")]
    public void CsvCacheLoader_HeadersFetched_ShouldBeCorrect(int index, string expected)
    {
        var result = _sut?.GetHeaders();

        result?[index].Should().Be(expected) ;
    }
    
    [Test]
    [TestCase(0, "Name")]
    [TestCase(1, "Age")]
    [TestCase(2, "MilesRun")]
    [TestCase(3, "PetsName")]
    public void CsvCacheLoader_DataStoreElementsFetched_FirstRowKeysShouldBeCorrect(int index, string expected)
    {
        var result = _sut?.UpdateCachedDataFromFile();

        result?[0].StoredValues[index].Key.Should().Be(expected) ;
    }
    
    [Test]
    [TestCase(0, "David")]
    [TestCase(1, "32")]
    [TestCase(2, "3.15")]
    [TestCase(3, "Whiskey")]
    public void CsvCacheLoader_DataStoreElementsFetched_FirstRowValuesShouldBeCorrect(int index, string expected)
    {
        var result = _sut?.UpdateCachedDataFromFile();

        result?[0].StoredValues[index].Value.Should().Be(expected) ;
    }
    
    [Test]
    [TestCase(0, "Name")]
    [TestCase(1, "Age")]
    [TestCase(2, "MilesRun")]
    [TestCase(3, "PetsName")]
    public void CsvCacheLoader_DataStoreElementsFetched_LastRowKeysShouldBeCorrect(int index, string expected)
    {
        var result = _sut?.UpdateCachedDataFromFile();

        result?[3].StoredValues[index].Key.Should().Be(expected) ;
    }
    
    [Test]
    [TestCase(0, "Dyamond")]
    [TestCase(1, "31")]
    [TestCase(2, "2.23")]
    [TestCase(3, "Trixie")]
    public void CsvCacheLoader_DataStoreElementsFetched_LastRowValuesShouldBeCorrect(int index, string expected)
    {
        var result = _sut?.UpdateCachedDataFromFile();

        result?[3].StoredValues[index].Value.Should().Be(expected) ;
    }
}
