using CommandsService.Models;

namespace CommandsService.Data;

public class CommandRepo : ICommandRepo
{
    private readonly AppDbContext dbContext;

    public CommandRepo(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public void CreateCommand(int platformId, Command command)
    {
        if(command == null) throw new ArgumentNullException("Command should be a valid command");

        command.PlatformId = platformId;

        dbContext.Commands.Add(command);
        this.SaveChanges();
    }

    public void CreatePlatform(Platform platform)
    {
        if(platform == null) throw new ArgumentNullException("Platform should be a valid platform");

        dbContext.Platforms.Add(platform);
        this.SaveChanges();
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return dbContext.Platforms;
    }

    public Command GetCommand(int platformId, int commandId)
    {
        return dbContext.Commands.FirstOrDefault(s=>s.PlatformId == platformId && s.Id == commandId);
    }

    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
    {
        return dbContext.Commands.Where(s => s.PlatformId == platformId);
    }

    public bool PlatformExist(int platformId)
    {
        return dbContext.Platforms.Any(p => p.Id == platformId);
    }

    public bool SaveChanges()
    {
        return dbContext.SaveChanges() > 0;
    }
}