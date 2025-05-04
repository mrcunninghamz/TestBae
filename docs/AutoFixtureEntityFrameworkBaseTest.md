# AutoFixture.EntityFramework.BaseTest<TSubject, TDbContext>

This base class combines the power of AutoFixture's automatic test data generation with Entity Framework testing capabilities. It provides a robust foundation for testing components that depend on both Entity Framework and other dependencies.

## Features

- Automatic test data generation using AutoFixture
- In-memory SQLite database setup and teardown
- Automatic transaction management for test isolation
- Integration with AutoFixture's customizations
- Support for both auto-mocking and real database contexts

## Usage

```csharp
public class UserServiceTests : AutoFixture.EntityFramework.BaseTest<UserService, MyDbContext>
{
    [Test]
    public async Task GetUser_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var userId = 1;
        var user = Fixture.Build<User>()
            .With(u => u.Id, userId)
            .Create();
        
        await DbContext.Users.AddAsync(user);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await Subject.GetUserAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(userId);
    }
}
```

## Setup

The base class handles the following setup automatically:

1. Creates a new SQLite in-memory database for each test
2. Configures AutoFixture with common customizations
3. Sets up the test subject with both mocked dependencies and the real database context
4. Manages database transactions for test isolation

## Properties

- `Subject`: The instance of the class being tested
- `DbContext`: The Entity Framework DbContext instance
- `Fixture`: AutoFixture instance for generating test data
- `MockRepository`: AutoMock repository for creating and verifying mocks

## Best Practices

1. Use the provided `Fixture` instance to create test data
2. Leverage transaction rollback for test isolation
3. Use AutoFixture's customization capabilities for complex scenarios
4. Take advantage of both mocked dependencies and real database operations