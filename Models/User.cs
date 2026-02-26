using Microsoft.VisualBasic;

namespace GameApi2.Models;

public class User
{
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "User";

    public DateTime Oprettet { get; set; } = DateTime.UtcNow;

    public DateTime Opdateret { get; set; } = DateTime.UtcNow;
}