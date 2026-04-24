namespace Thinka.Domain.Dto.User;

public class UserProfileDto
{
    public string UserName { get; set; } = null!;
    
    public string Email { get; set; } = null!;

    public int IdeasCount { get; set; }

    public int LikesCount { get; set; }

    public int SavesCount { get; set; }

    public int FollowersCount { get; set; }

    public int FollowingCount { get; set; }
}
