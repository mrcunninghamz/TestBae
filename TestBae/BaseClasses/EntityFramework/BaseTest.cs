using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestBae.BaseClasses.EntityFramework;

public abstract class BaseTest<TSubject, TDbContext> : BaseTest<TSubject>
    where TSubject : class where TDbContext : DbContext
{
    protected TDbContext DbContext => ServiceProvider.GetService<TDbContext>();

    protected BaseTest() : base()
    {
        DbContext.Database.EnsureCreated();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddDbContext<TDbContext>((_, optionsBuilder) =>
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            optionsBuilder.UseSqlite(connection);
        });
    }
}