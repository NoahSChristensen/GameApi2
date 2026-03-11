using System.ComponentModel.DataAnnotations;


namespace GameApi2.DTOs;


public class PutPointDto
{
    [Range(0, long.MaxValue, ErrorMessage = "Total skal være 0 eller derover.")]
    public long Total { get; set; }
}