using Microsoft.AspNetCore.Mvc;
using WebApplicationDBF.DTOs;
using WebApplicationDBF.Exceptions;
using WebApplicationDBF.Services;

namespace WebApplicationDBF.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(IPatientService patientService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, CancellationToken cancellationToken)
    {
        return Ok(await patientService.GetAllAsync(search, cancellationToken));
    }

    [HttpPost("{pesel}/bedassignments")]
    public async Task<IActionResult> AddBedAssignmentAsync([FromRoute] string pesel, [FromBody] BedAssignmentRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            await patientService.AddBedAssignmentAsync(pesel, request, cancellationToken);
            return Created();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
