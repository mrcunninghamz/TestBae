using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Community.AutoMapper;
using EntityFrameworkCore.AutoFixture.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TestBae.BaseClasses.AutoFixture.EntityFramework;

public class BaseTest<TSubject, TDbContext> : BaseTest<TSubject> where TSubject : class where TDbContext : DbContext
{
    public TDbContext DbContext { get; set; }
    
    protected override void ConfigureFixture()
    {
        Fixture.Customize(new CompositeCustomization(
                new AutoMoqCustomization(), 
                new AutoMapperCustomization(x => x.AddMaps(typeof(TSubject))),
                new SqliteCustomization()
            )
        );

        DbContext = Fixture.Freeze<TDbContext>();
    }
}