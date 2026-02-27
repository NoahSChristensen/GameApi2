namespace GameApi2.DTOs;

using GameApi2.DTOs;
using GameApi2.Models;

public class OrderGetDto
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public User? User { get; set; }

    public string OrderValue { get; set; } = string.Empty;

    public DateTime Oprettet { get; set; } = DateTime.UtcNow;

    public DateTime Opdateret { get; set; } = DateTime.UtcNow;


}