using System.ComponentModel.DataAnnotations;

namespace GameApi2.DTOs;

public class UserPostDto
{

    [Required]
    [EmailAddress(ErrorMessage = "Ugyldig Email adresse")]

    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]

    public string Name { get; set; } = string.Empty;

}

