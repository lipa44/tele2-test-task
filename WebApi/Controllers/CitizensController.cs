namespace WebApi.Controllers;

using Services.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Dto;
using Extensions;
using Extensions.CitizensFilters;
using Filters;
using Models;

[Route("api/[controller]")]
[ApiController]
public class CitizensController : ControllerBase
{
    private readonly ICitizenService _citizenService;

    public CitizensController(ICitizenService citizenService)
        => _citizenService = citizenService;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IndexViewModel>> GetCitizens(
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CitizenFullDto>> GetCitizenById(
        [FromRoute] Guid citizenId)
    {
        var citizen = await _citizenService.FindCitizenByIdAsync(citizenId);

        if (citizen is null) return NotFound();

        return Ok(citizen.ToFullDto());
    }
}