using System.ComponentModel.DataAnnotations;

namespace GameApi2.DTOs;

public class RegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(2)]
    public string Name { get; set; } = string.Empty;

    [Required, MinLength(6, ErrorMessage = "Password skal mindst være 6 gen")]
    public string Password { get; set; } = string.Empty;
}