using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PreparePopulation(IApplicationBuilder builder)
    {
        using var serviceScope = builder.ApplicationServices.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();

        SeedData(dbContext);
    }

    private static void SeedData(AppDbContext dbContext)
    {
        if (dbContext.Platforms.Any()) return;

        dbContext.Platforms.AddRange(
                        new Platform()
                        {
                            Id = 1,
                            Name = "DotNet",
                            Publisher = "Microsoft",
                            Cost = "$10.00",
                        },
                        new Platform()
                        {
                            Id = 2,
                            Name = "Sql Server Express",
                            Publisher = "Microsoft",
                            Cost = "$15.00",
                        },
                        new Platform()
                        {
                            Id = 3,
                            Name = "Gmail",
                            Publisher = "Google",
                            Cost = "$5.00",
                        }
            );

        dbContext.SaveChanges();
    }
}