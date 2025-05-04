# EntityFramework.BaseTest<TSubject>

The `EntityFramework.BaseTest<TSubject>` class is a specialized base test class designed for testing components that rely on Entity Framework Core. It provides an in-memory SQLite database context and transaction management for your tests.

## Features

- Automatic setup of in-memory SQLite database
- Transaction management for test isolation
- Simplified database context configuration
- Built-in cleanup after each test

## Usage

```csharp
public class UserServiceTests : EntityFramework.BaseTest<UserService>
{
    private AppDbContext _dbContext;

    protected override void ConfigureServices(IServiceCollection services)
    {
        // Configure your database context
        _dbContext = CreateDbContext<AppDbContext>(options =>
        {
            // Add any additional configuration
            options.UseSqlite(Connection);
        });

        // Register your database context
        services.AddScoped(_ => _dbContext);

        // Register other dependencies
        services.AddScoped<IUserRepository, UserRepository>();
    }

    [Fact]
    public async Task CreateUser_ValidData_UserCreated()
    {
        // Arrange
        var user = new User { Name = "John Doe", Email = "john@example.com" };

        // Act
        await Subject.CreateUserAsync(user);

        // Assert
        var savedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        Assert.NotNull(savedUser);
        Assert.Equal(user.Name, savedUser.Name);
    }
}
```

## Key Components

### CreateDbContext<TContext>

Creates a new database context with the provided configuration:

```csharp
protected TContext CreateDbContext<TContext>(Action<DbContextOptionsBuilder> configure = null)
```

### Transaction Management

Each test runs within a transaction that is automatically rolled back after the test completes, ensuring test isolation.

### Connection Property

Provides access to the SQLite database connection:

```csharp
protected SqliteConnection Connection { get; }
```

## Best Practices

1. Use the `ConfigureServices` method to set up your database context and register dependencies
2. Utilize the `CreateDbContext` method to create properly configured database contexts
3. Keep database operations within the test methods to ensure proper transaction management
4. Don't manually manage transactions - the base class handles this automatically
5. Use async/await for database operations to follow Entity Framework best practices

## Example with Complex Configuration

```csharp
public class OrderProcessingTests : EntityFramework.BaseTest<OrderProcessor>
{
    private AppDbContext _dbContext;
    private IMapper _mapper;

    protected override void ConfigureServices(IServiceCollection services)
    {
        // Configure database
        _dbContext = CreateDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(Connection);
            options.EnableSensitiveDataLogging();
        });

        // Configure AutoMapper
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OrderMappingProfile>();
        });
        _mapper = configuration.CreateMapper();

        // Register services
        services.AddScoped(_ => _dbContext);
        services.AddSingleton(_mapper);
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPaymentService, PaymentService>();
    }

    [Fact]
    public async Task ProcessOrder_ValidOrder_OrderProcessedSuccessfully()
    {
        // Arrange
        var order = new Order
        {
            CustomerId = 1,
            Items = new List<OrderItem>
            {
                new() { ProductId = 1, Quantity = 2 }
            }
        };

        await _dbContext.Customers.AddAsync(new Customer { Id = 1, Name = "Test Customer" });
        await _dbContext.Products.AddAsync(new Product { Id = 1, Name = "Test Product", Price = 10.00m });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await Subject.ProcessOrderAsync(order);

        // Assert
        Assert.True(result.Success);
        var savedOrder = await _dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == result.OrderId);
        Assert.NotNull(savedOrder);
        Assert.Equal(order.CustomerId, savedOrder.CustomerId);
        Assert.Equal(order.Items.Count, savedOrder.Items.Count);
    }
}
```