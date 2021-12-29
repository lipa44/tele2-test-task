namespace WebApi.Models;

using Dto;

public record IndexViewModel
{
    public IEnumerable<CitizenDto> Citizens { get; init; }
    public PageViewModel PageViewModel { get; init; }
}