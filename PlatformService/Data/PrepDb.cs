using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PreparePopulation(IApplicationBuilder builder, bool isProd)
    {
        using var serviceScope = builder.ApplicationServices.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();

        SeedData(dbContext, isProd);
    }

    private static void SeedData(AppDbContext dbContext, bool isProd)
    {
        if (isProd)
        {
            try
            {
                dbContext.Database.Migrate();
            }
            catch (System.Exception)
            {
                Console.WriteLine("--> There was an error on migrating db");
                throw;
            }
            
        }

        if (dbContext.Platforms.Any()) return;

        dbContext.Platforms.AddRange(
                        new Platform()
                        {
                            Name = "DotNet",
                            Publisher = "Microsoft",
                            Cost = "$10.00",
                        },
                        new Platform()
                        {
                            Name = "Sql Server Express",
                            Publisher = "Microsoft",
                            Cost = "$15.00",
                        },
                        new Platform()
                        {
                            Name = "Gmail",
                            Publisher = "Google",
                            Cost = "$5.00",
                        }
            );

        dbContext.SaveChanges();
    }
}