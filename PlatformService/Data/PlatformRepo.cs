using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepo : IPlatformRepo
{
    private readonly AppDbContext context;

    public PlatformRepo(AppDbContext context)
    {
        this.context = context;
    }
    public void CreatePlatform(Platform platform)
    {
        if(platform == null) throw new ArgumentNullException("Platform can not be null");

        this.context.Platforms.Add(platform);
        this.context.SaveChanges();
    }

    public IEnumerable<Platform> GetAllPlatforms() => this.context.Platforms;

    public Platform GetPlatformByid(int id) => this.context.Platforms.First(p => p.Id == id);

    public bool SaveChanges() => this.context.SaveChanges() >= 0;
}