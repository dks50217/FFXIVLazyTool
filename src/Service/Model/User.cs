using System;
using System.Collections.Generic;

namespace Core.Model;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? RefreshToken { get; set; }

    public string? TotpKey { get; set; }
}
