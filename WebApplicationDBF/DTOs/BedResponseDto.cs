namespace WebApplicationDBF.DTOs;

public record BedResponseDto(
    int Id,
    BedTypeResponseDto BedType,
    RoomResponseDto Room);
