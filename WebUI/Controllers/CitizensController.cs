namespace WebUI.Controllers;

using Extensions.CitizensFilters;
using Filters;
using Domain.Enums;
using Dto;
using Extensions;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

[Route("api/[controller]")]
[ApiController]
public class CitizensController : ControllerBase
{
    private readonly ICitizenService _citizenService;

    public CitizensController(ICitizenService citizenService)
        => _citizenService = citizenService;

    [HttpGet]
    public async Task<OkObjectResult> GetCitizens(
        [FromQuery] CitizenSex? citizenSex,
        [FromQuery] int ageRangeStart,
        [FromQuery] int ageRangeEnd,
        [FromQuery] int takeAmount,
        [FromQuery] int pageNumber)
    {
        var citizens = await _citizenService.GetCitizensAsync();
        PaginationFilter paginationFilter = new(takeAmount, pageNumber);

        citizens
            .UseAgeFilter(new(ageRangeStart, ageRangeEnd))
            .UseSexFilter(citizenSex);

        return Ok(new IndexViewModel
        {
            PageViewModel = new (citizens.Count, paginationFilter.Page, paginationFilter.Take),
            Citizens = citizens.UsePaginationFilter(paginationFilter).Select(c => c.ToDto()), 
        });
    }

    [HttpGet("{citizenId}")]
    public async Task<ActionResult<IReadOnlyCollection<CitizenFullDto>>> GetCitizenById(
        [FromRoute] Guid citizenId)
        => Ok((await _citizenService.GetCitizenByIdAsync(citizenId)).ToFullDto());
}