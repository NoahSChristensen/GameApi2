using System.ComponentModel.DataAnnotations;

namespace GameApi2.DTOs;

public class UserPatchDto
{
    [EmailAddress] public string? Email { get; set; }

    [MinLength(2)] public string? Name { get; set; }
}