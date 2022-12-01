using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[ApiController]
[Route("api/c/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepo repository;
    private readonly IMapper mapper;

    public PlatformsController(ICommandRepo repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAllPlatforms()
    {
        Console.WriteLine("--> Getting platforms from CommandsService");

        var platforms = this.repository.GetAllPlatforms();
        var items = this.mapper.Map<IEnumerable<PlatformReadDto>>(platforms);

        return Ok(items);
    }

    [HttpPost]
    public IActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound POST # Command Servie");
        return Ok("Inbound test of Platforms Controller");
    }
}