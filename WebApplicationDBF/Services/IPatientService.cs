using WebApplicationDBF.DTOs;

namespace WebApplicationDBF.Services;

public interface IPatientService
{
    Task<ICollection<PatientResponseDto>> GetAllAsync(string? search, CancellationToken cancellationToken);
    Task AddBedAssignmentAsync(string pesel, BedAssignmentRequestDto request, CancellationToken cancellationToken);
}
