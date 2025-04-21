using System;
using TestBae.BaseClasses;
using TestBae.Tests.Mocks;
using Moq;
using Xunit;

namespace TestBae.Tests.BaseClasses;

public class BaseTestTests : BaseTest<SeedService>
{
    private Mock<ISeededTestValue> _seededValue;

    protected override void ConfigureServices(IServiceCollection services)
    {
        // Create mocks
        _seededValue = new Mock<ISeededTestValue>();
        
        // Add to dependency injection
        services.AddSingleton(_seededValue.Object);
        services.AddSingleton<SeededTestValueResolver>();
        
        //Run base configuration
        base.ConfigureServices(services);
    }

    [Fact]
    public void Mapper_ValidSetup_MapsSeed()
    {
        // Arrange
        var seed = Guid.NewGuid().ToString();
            
        var test = new Test
        {
            Name = "Mapper Test"
        };

        _seededValue.Setup(x => x.Value).Returns(seed);
            
        // Act
        var result = TestSubject.TestMethod(test);
            
        // Assert
        Assert.NotNull(result);
        Assert.Equal(seed, result.Random);
    }
}