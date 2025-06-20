using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Smart_Medical.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class Smart_MedicalDbContextFactory : IDesignTimeDbContextFactory<Smart_MedicalDbContext>
{
    public Smart_MedicalDbContext CreateDbContext(string[] args)
    {
        Smart_MedicalEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<Smart_MedicalDbContext>()
            .UseMySql(configuration.GetConnectionString("Default"),ServerVersion.Parse("8.0.42-mysql"));

        return new Smart_MedicalDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory()) // 这将正确指向 BookStore.HttpApi.Host 的输出目录
          .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
