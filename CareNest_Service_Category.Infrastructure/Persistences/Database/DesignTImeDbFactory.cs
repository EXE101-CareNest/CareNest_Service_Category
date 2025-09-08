﻿using CareNest_Service_Category.Infrastructure.Persistences.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CareNest_Service_Category.Infrastructure.Persistences.Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "CareNest_Service_Category.API");
            apiProjectPath = Path.GetFullPath(apiProjectPath);

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.Development.json", optional: false)
                .Build();

            var dbSettings = new DatabaseSettings();
            configuration.GetSection("DatabaseSettings").Bind(dbSettings);

            string connectionString = dbSettings.GetConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new DatabaseContext(optionsBuilder.Options);
        }

    }

}
