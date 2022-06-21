using FluentAssertions;
using NUnit.Framework;

namespace CollectionConfig.net.UnitTests;

public class DummyTests
{
    // private _sut;
    //
    // [SetUp]
    // public void SetUp()
    // {
    //     _sut = new();
    // }
    //
    // [TearDown]
    // public void TearDown()
    // {
    //     _sut = null;
    // }
    
    [Test]
    public void DUMMYTEST_Test_Test()
    {
        var result = true;
    
        result.Should().Be(true);
    }
}