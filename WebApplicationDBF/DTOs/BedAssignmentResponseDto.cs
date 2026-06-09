namespace WebApplicationDBF.DTOs;

public record BedAssignmentResponseDto(
    int Id,
    DateTime From,
    DateTime? To,
    BedResponseDto Bed);
