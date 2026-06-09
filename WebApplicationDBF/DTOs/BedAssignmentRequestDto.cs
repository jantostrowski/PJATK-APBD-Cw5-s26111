namespace WebApplicationDBF.DTOs;

public record BedAssignmentRequestDto(
    DateTime From,
    DateTime? To,
    string BedType,
    string Ward);
