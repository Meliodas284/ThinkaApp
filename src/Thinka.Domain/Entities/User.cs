using System;
using System.Collections.Generic;
using Thinka.Domain.Interfaces.Common;

namespace Thinka.Domain.Entities;

public class User : IAuditable
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public byte[] PasswordHash { get; set; } = [];

    public byte[] PasswordSalt { get; set; } = [];

    public string RefreshToken { get; set; } = string.Empty;

    public DateTime TokenCreated { get; set; }

    public DateTime TokenExpires { get; set; }
    
    public ICollection<Idea> Ideas { get; set; } = new List<Idea>();

    public ICollection<Like> Likes { get; set; } = new List<Like>();

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<Save> Saves { get; set; } = new List<Save>();

    public ICollection<Follow> Following { get; set; } = new List<Follow>();

    public ICollection<Follow> Followers { get; set; } = new List<Follow>();

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
