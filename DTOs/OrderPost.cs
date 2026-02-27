using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GameApi2.DTOs;

public class OrderPostDTO
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public int OrderValue { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

}