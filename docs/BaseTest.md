# BaseTest<TSubject>

This is an abstract class used in unit tests to automatically setup AutoMapper and inject your mocks into dependency injection.

## Setup
In your unit test class reference the class you are testing. In this example the `PermissionsController` is the class being tested.

```c#
public class PermissionsControllerTests : BaseTest<PermissionsController>
```

The `BaseTest` class will look for AutoMapper profiles in the assembly the `PermissionsController` comes from.

## Configuration

Override the `ConfigureServices` method to set up your dependencies:

```c#
protected override void ConfigureServices(IServiceCollection services)
{
    _mockExternalEntityManager = new Mock<IExternalEntityManager>();
    _microsoftGraphUserAdapterMock = new Mock<IMicrosoftGraphUserAdapter>();
    _mockLogger = new Mock<ILogger<PermissionsController>>();

    //these are mocks for the controller
    services.AddTransient(_ => _mockExternalEntityManager.Object);
    services.AddTransient(_ => _microsoftGraphUserAdapterMock.Object);
    services.AddTransient(_ => _mockLogger.Object);

    //this is stuff that AutoMapper cares about
    _httpcontext = new DefaultHttpContext();
    var _httpContextAccessor = new Mock<IHttpContextAccessor>();
    _httpContextAccessor
        .Setup(x => x.HttpContext)
        .Returns(_httpcontext);

    services.AddSingleton(_httpContextAccessor.Object);
    services.AddSingleton<PagingResponseConverter<ExternalEntity, ExternalEntity>>();

    base.ConfigureServices(services);
}
```

## Properties

- `TestSubject` - This is the class you are testing, instantiated by the base class with the dependencies that were configured.
- `Mapper` - An instance of AutoMapper setup by the base class with the dependencies that were configured.
- `ServiceProvider` - The service provider with all the dependencies.

## Example Usage

Here's a complete example test class using the `BaseTest`:

```c#
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TestBae.BaseClasses;
using Xunit;

public class UserServiceTests : BaseTest<UserService>
{
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<ILogger<UserService>> _mockLogger;

    protected override void ConfigureServices(IServiceCollection services)
    {
        // Create mocks
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        
        // Add mocks to dependency injection
        services.AddTransient(_ => _mockUserRepository.Object);
        services.AddTransient(_ => _mockLogger.Object);
        
        // Call base configuration
        base.ConfigureServices(services);
    }

    [Fact]
    public async Task GetUserById_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var userId = 1;
        var user = new User { Id = userId, Name = "John Doe" };
        _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await TestSubject.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("John Doe", result.Name);
    }
}
```