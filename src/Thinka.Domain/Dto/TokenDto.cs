using System.ComponentModel.DataAnnotations;

namespace Thinka.Domain.Dto;

public class TokenDto
{
    [Required]
    public required string AccessToken { get; set; }

    [Required]
    public required string RefreshToken { get; set; }
}
