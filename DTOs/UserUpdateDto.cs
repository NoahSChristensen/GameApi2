using System.ComponentModel.DataAnnotations;

namespace GameApi2.DTOs;


public class UserUpdateDto
{
    [Required][EmailAddress] public string Email { get; set; } = string.Empty;

    [Required][MinLength(2)] public string Name { get; set; } = string.Empty;
}