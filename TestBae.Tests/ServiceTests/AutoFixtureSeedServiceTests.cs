using System;
using AutoFixture;
using Moq;
using TestBae.Tests.Mocks;
using Xunit;

namespace TestBae.Tests.ServiceTests;

public class AutoFixtureSeedServiceTests : TestBae.BaseClasses.AutoFixture.BaseTest<SeedService>
{
    private Mock<ISeededTestValue> _seededValue;

    /// <summary>
    /// Configures the AutoFixture instance used for generating test data.
    /// This method can be overridden to customize the behavior of the fixture
    /// for the specific needs of a derived test class.
    /// </summary>
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