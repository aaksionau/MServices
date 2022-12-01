using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepo repository;
    private readonly IMapper mapper;

    public CommandsController(ICommandRepo repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetCommandsForPlatform(int platformId)
    {
        Console.WriteLine($"--> Get all commands for the platform: {platformId}");

        var commands = this.repository.GetCommandsForPlatform(platformId);
        var items = mapper.Map<IEnumerable<CommandReadDto>>(commands);

        return Ok(items);
    }

    [HttpGet("{commandId:int}", Name = "GetCommandForPlatform")]
    public IActionResult GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"--> Get command for the platform: {platformId}, command: {commandId}");

        var command = this.repository.GetCommand(platformId, commandId);
        var item = this.mapper.Map<CommandReadDto>(command);

        return Ok(item);
    }

    [HttpPost]
    public IActionResult CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
    {
        Console.WriteLine($"--> Creation of the command for platform: {platformId}");

        if (!repository.PlatformExist(platformId))
        {
            return NotFound();
        }

        var command = this.mapper.Map<Command>(commandDto);
        this.repository.CreateCommand(platformId, command);

        var commandReadDto = this.mapper.Map<CommandReadDto>(command);
        return CreatedAtRoute(nameof(GetCommandForPlatform),
                                new { platformId = platformId, commandId = commandReadDto.Id },
                                commandReadDto);
    }
}