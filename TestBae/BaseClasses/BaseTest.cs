using System.Reflection;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace TestBae.BaseClasses;

public abstract class BaseTest<TSubject> where TSubject : class
{
    protected IMapper Mapper => ServiceProvider.GetService<IMapper>();
        
    protected TSubject TestSubject => ServiceProvider.GetService<TSubject>();
        
    protected ServiceProvider ServiceProvider;

    protected BaseTest() => SetupProgram();

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(TSubject)));
        services.AddSingleton<TSubject>();
    }
        
    private void SetupProgram()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public void TestMaps() => Mapper.ConfigurationProvider.AssertConfigurationIsValid();
}