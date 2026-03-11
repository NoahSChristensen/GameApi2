namespace GameApi2.DTOs;

public class MazeGetDto
{
    public int Id { get; set; }

    public int levelId { get; set; }

    public string UserId { get; set; } = string.Empty;
}