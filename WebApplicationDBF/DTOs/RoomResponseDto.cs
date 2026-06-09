namespace WebApplicationDBF.DTOs;

public record RoomResponseDto(
    string Id,
    bool HasTv,
    WardResponseDto Ward);
