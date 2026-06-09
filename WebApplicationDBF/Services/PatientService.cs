using Microsoft.EntityFrameworkCore;
using WebApplicationDBF.DTOs;
using WebApplicationDBF.Exceptions;
using WebApplicationDBF.Infrastructure;
using WebApplicationDBF.Models;

namespace WebApplicationDBF.Services;

public class PatientService(DbfirstContext ctx) : IPatientService
{
    public async Task<ICollection<PatientResponseDto>> GetAllAsync(string? search, CancellationToken cancellationToken)
    {
        return await ctx.Patients
            .Where(patient => search == null ||
                              EF.Functions.Like(patient.FirstName, $"%{search}%") ||
                              EF.Functions.Like(patient.LastName, $"%{search}%"))
            .Select(patient => new PatientResponseDto(
                patient.Pesel,
                patient.FirstName,
                patient.LastName,
                patient.Age,
                patient.Sex ? "Male" : "Female",
                patient.Admissions.Select(admission => new AdmissionResponseDto(
                    admission.Id,
                    admission.AdmissionDate,
                    admission.DischargeDate,
                    new WardResponseDto(
                        admission.Ward.Id,
                        admission.Ward.Name,
                        admission.Ward.Description)))
                    .ToList(),
                patient.BedAssignments.Select(assignment => new BedAssignmentResponseDto(
                    assignment.Id,
                    assignment.From,
                    assignment.To,
                    new BedResponseDto(
                        assignment.Bed.Id,
                        new BedTypeResponseDto(
                            assignment.Bed.BedType.Id,
                            assignment.Bed.BedType.Name,
                            assignment.Bed.BedType.Description),
                        new RoomResponseDto(
                            assignment.Bed.Room.Id,
                            assignment.Bed.Room.HasTv,
                            new WardResponseDto(
                                assignment.Bed.Room.Ward.Id,
                                assignment.Bed.Room.Ward.Name,
                                assignment.Bed.Room.Ward.Description)))))
                    .ToList()))
            .ToListAsync(cancellationToken);
    }

    public async Task AddBedAssignmentAsync(string pesel, BedAssignmentRequestDto request, CancellationToken cancellationToken)
    {
        if (!await ctx.Patients.AnyAsync(patient => patient.Pesel == pesel, cancellationToken))
        {
            throw new NotFoundException($"Patient with PESEL '{pesel}' was not found.");
        }

        var bed = await ctx.Beds.FirstOrDefaultAsync(bed =>
            bed.BedType.Name == request.BedType &&
            bed.Room.Ward.Name == request.Ward &&
            !bed.BedAssignments.Any(assignment =>
                (request.To == null || assignment.From < request.To) &&
                (assignment.To == null || request.From < assignment.To)),
            cancellationToken);

        if (bed == null)
        {
            throw new NotFoundException(
                $"No available bed of type '{request.BedType}' was found in ward '{request.Ward}' for the specified period.");
        }

        var assignment = new BedAssignment
        {
            PatientPesel = pesel,
            BedId = bed.Id,
            From = request.From,
            To = request.To
        };

        ctx.BedAssignments.Add(assignment);
        await ctx.SaveChangesAsync(cancellationToken);
    }
}
