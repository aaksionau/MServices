using System.ComponentModel.DataAnnotations;

namespace PlatformService.Dtos;

public class PlatformWriteDto
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Publisher { get; set; }
    public required string Cost { get; set; }
}