namespace WebApplicationDBF.DTOs;

public record PatientResponseDto(
    string Pesel,
    string FirstName,
    string LastName,
    int Age,
    string Sex,
    IEnumerable<AdmissionResponseDto> Admissions,
    IEnumerable<BedAssignmentResponseDto> BedAssignments);
