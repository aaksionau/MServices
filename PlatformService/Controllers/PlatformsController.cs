using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo platformRepo;
    private readonly IMapper mapper;
    private readonly ICommandDataClient commandDataClient;

    public PlatformsController(
        IPlatformRepo platformRepo, 
        IMapper mapper,
        ICommandDataClient commandDataClient)
    {
        this.platformRepo = platformRepo;
        this.mapper = mapper;
        this.commandDataClient = commandDataClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
    {
        var items = this.platformRepo.GetAllPlatforms();
        return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(items));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        var platform = this.platformRepo.GetPlatformByid(id);
        return Ok(mapper.Map<PlatformReadDto>(platform));
    }

    [HttpPost]
    public async Task<ActionResult> CreatePlatform(PlatformWriteDto platformDto)
    {
        var platform = mapper.Map<Platform>(platformDto);
        this.platformRepo.CreatePlatform(platform);

        var platformReadDto = mapper.Map<PlatformReadDto>(platform);
       
        await this.commandDataClient.SendPlaformToCommand(platformReadDto);
       
        return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformDto);
    }
}