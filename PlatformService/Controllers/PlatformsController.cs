using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo platformRepo;
    private readonly IMapper mapper;

    public PlatformsController(IPlatformRepo platformRepo, IMapper mapper)
    {
        this.platformRepo = platformRepo;
        this.mapper = mapper;
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
    public ActionResult CreatePlatform(PlatformWriteDto platformDto)
    {
        var platform = mapper.Map<Platform>(platformDto);
        this.platformRepo.CreatePlatform(platform);

        var platformReadDto = mapper.Map<Platform>(platform);

        return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformDto);
    }
}