namespace WebApplicationDBF.DTOs;

public record AdmissionResponseDto(
    int Id,
    DateTime AdmissionDate,
    DateTime? DischargeDate,
    WardResponseDto Ward);