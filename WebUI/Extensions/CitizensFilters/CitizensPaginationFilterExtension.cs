namespace WebUI.Extensions.CitizensFilters;

using Domain.Entities;
using Filters;

public static class CitizensPaginationFilterExtension
{
    public static List<Citizen> UsePaginationFilter(this List<Citizen> citizens, PaginationFilter paginationFilter)
        => citizens
            .Skip((paginationFilter.Page - 1) * paginationFilter.Take)
            .Take(paginationFilter.Take).ToList();
}