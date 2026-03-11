using System.ComponentModel.DataAnnotations;


namespace GameApi2.DTOs;


public class PostPointRequest
{
    [Required(ErrorMessage = "Source er påkrævet")]
    public string? Source { get; set; }

    [Range(1, 100, ErrorMessage = "Amount skal være mellem 1 og 100")]
    public int Amount { get; set; }
}