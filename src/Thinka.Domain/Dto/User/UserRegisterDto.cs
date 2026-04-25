using System.ComponentModel.DataAnnotations;

namespace Thinka.Domain.Dto.User;

public class UserRegisterDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MaxLength(100)]
    public required string UserName { get; set; }

    [Required]
    [MinLength(8)]
    public required string Password { get; set; }
}
