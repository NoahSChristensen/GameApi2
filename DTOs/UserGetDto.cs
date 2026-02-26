namespace GameApi2.DTOs;

public class UserGetDto
{
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public DateTime Oprettet { get; set; }

    public DateTime Opdateret { get; set; }
}