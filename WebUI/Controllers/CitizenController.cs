namespace WebUI.Controllers;

using Domain.Entities;
using Infrastructure.Interfaces;
using Dto;
using Extensions;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CitizenController : ControllerBase
{
    private readonly ICitizenService _citizenService;

    public CitizenController(ICitizenService citizenService)
        => _citizenService = citizenService;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CitizenDto>>> GetCitizens()
        => Ok(_citizenService.GetAllCitizensAsync().Result.Select(c => c.ToDto()));

    [HttpGet("{citizenId}")]
    public async Task<ActionResult<IReadOnlyCollection<CitizenFullDto>>> GetCitizenById(
        [FromRoute] Guid citizenId)
        => Ok(_citizenService.GetCitizenByIdAsync(citizenId).Result.ToFullDto());

    [HttpPut]
    public async Task<ActionResult<IReadOnlyCollection<CitizenFullDto>>> GetCitizenById(
        [FromBody] Citizen citizen)
        => Ok(_citizenService.RegisterCitizen(citizen).Result.ToFullDto());
}