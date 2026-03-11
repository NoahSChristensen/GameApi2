namespace GameApi2.DTOs;


public record AddPointsResultDto(string UserId, long Total, long Added, string Source);