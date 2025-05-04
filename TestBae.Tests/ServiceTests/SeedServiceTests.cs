using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TestBae.BaseClasses;
using TestBae.Tests.Mocks;
using Xunit;

namespace TestBae.Tests.ServiceTests;

/// <summary>
/// Contains unit tests for the SeedService class.
/// </summary>
/// <remarks>
/// This class is responsible for testing the behavior of the SeedService and its dependencies,
/// including mapping configurations and data resolution logic.
/// It inherits from the BaseTest class, which provides a test setup and configuration logic for the service being tested.
/// </remarks>
public class SeedServiceTests : BaseTest<SeedService>
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