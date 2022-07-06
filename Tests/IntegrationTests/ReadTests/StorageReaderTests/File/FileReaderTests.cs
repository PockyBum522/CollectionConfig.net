using System;
using System.IO;
using CollectionConfig.net.Core.Interfaces;
using CollectionConfig.net.Logic.StorageReaders.File;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Serilog;

namespace IntegrationTests.ReadTests.StorageReadersTests.File;

public class FileReaderTests
{
    private const string ExamplePersonListCsvPath = @"TestResources\ExamplePerson\ExamplePersonList.csv";
    
    // ReSharper disable once FieldCanBeMadeReadOnly.Local because if we uncomment logging below it won't be readonly
    private ILogger? _testLogger;
    private FileStoreReader? _sut;
    
    [SetUp]
    public void SetUp()
    {
        // Uncomment these if you want logging. Debugging seems potentially iffy on proxy objects, so logging may help:
        var logPath =
            Path.Join(Environment.GetFolderPath(
                Environment.SpecialFolder.Desktop), 
                "CollectionConfig.CsvEnumerationTests.net.log"); 
        
        _testLogger ??= TestLogger.GetTestLogger(logPath);

        _sut = new FileStoreReader(ExamplePersonListCsvPath);
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
    public void CsvCacheLoader_HeadersFetched_ShouldBeCorrect()
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
