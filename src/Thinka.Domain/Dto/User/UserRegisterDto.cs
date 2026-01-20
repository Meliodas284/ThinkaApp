namespace Thinka.Domain.Dto.User;

public class UserRegisterDto
{
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}
