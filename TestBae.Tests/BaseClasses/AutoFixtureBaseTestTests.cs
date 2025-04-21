using System;
using AutoFixture;
using TestBae.BaseClasses;
using TestBae.BaseClasses.AutoFixture;
using TestBae.Tests.Mocks;
using Moq;
using Xunit;

namespace TestBae.Tests.BaseClasses;

public class AutoFixtureBaseTestTests : TestBae.BaseClasses.AutoFixture.BaseTest<SeedService>
{
    private Mock<ISeededTestValue> _seededValue;


    protected override void ConfigureFixture()
    {
        // Create mocks
        _seededValue = Fixture.Freeze<Mock<ISeededTestValue>>();
        Fixture.Freeze<SeededTestValueResolver>();
    }

    [Fact]
    public void Mapper_ValidSetup_MapsSeed()
    {
        // Arrange
        var seed = Guid.NewGuid().ToString();

        var test = Fixture.Create<Test>();

        _seededValue.Setup(x => x.Value).Returns(seed);
            
        // Act
        var result = TestSubject.TestMethod(test);
            
        // Assert
        Assert.NotNull(result);
        Assert.Equal(seed, result.Random);
    }
}