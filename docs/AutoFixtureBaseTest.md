# AutoFixture.BaseTest<TSubject>

This base class uses AutoFixture to simplify test setup and data generation. It's particularly useful for tests that require complex object graphs or when you want to reduce boilerplate test setup code.

## Setup

First, reference the class you're testing:

```c#
public class UserServiceTests : TestBae.BaseClasses.AutoFixture.BaseTest<UserService>
```

## Configuration

Override the `ConfigureFixture` method to set up your test dependencies:

```c#
protected override void ConfigureFixture()
{
    // Create and configure mocks
    var mockUserRepository = Fixture.Freeze<Mock<IUserRepository>>();
    
    // No need to manually register dependencies - AutoFixture does this for you
}
```

The key difference from BaseTest is that you don't need to manually set up the dependency injection container. AutoFixture does this for you using its `Freeze<T>` method, which both creates an instance and registers it with the fixture.

## Properties

- `TestSubject` - The class being tested, automatically instantiated by AutoFixture.
- `Mapper` - An AutoMapper instance configured with mappings from the assembly of the class being tested.
- `Fixture` - The AutoFixture instance you can use to create test data.

## Example Usage

Here's a complete example test class using `AutoFixture.BaseTest`:

```c#
using AutoFixture;
using Moq;
using TestBae.BaseClasses.AutoFixture;
using Xunit;

public class UserServiceTests : TestBae.BaseClasses.AutoFixture.BaseTest<UserService>
{
    private Mock<IUserRepository> _mockUserRepository;

    protected override void ConfigureFixture()
    {
        // Create and configure mocks
        _mockUserRepository = Fixture.Freeze<Mock<IUserRepository>>();
        
        // AutoFixture automatically handles dependency injection
    }

    [Fact]
    public async Task GetUserById_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var userId = 1;
        var user = Fixture.Create<User>(); // AutoFixture creates a fully populated User
        user.Id = userId;
        
        _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await TestSubject.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal(user.Name, result.Name);
    }
}
```

## When to Use

Use AutoFixture.BaseTest when:
- You want to reduce boilerplate code in your tests
- You need to create complex object graphs for testing
- You want to focus on the specific test behavior rather than setup code
- You prefer automatic test data generation over manual setup

The class automatically validates AutoMapper configurations via the built-in `TestMaps()` test method that runs with each test class.