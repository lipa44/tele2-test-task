namespace WebUI.Extensions.CitizensFilters;

using Domain.Entities;
using Filters;

public static class CitizensAgeFilterExtension
{
    public static List<Citizen> UseAgeFilter(this List<Citizen> citizens, AgeRangeFilter ageFilter)
    {
        if (ageFilter.IsValid)
            citizens.RemoveAll(c => c.Age < ageFilter.Start || c.Age > ageFilter.End);

        return citizens;
    }
}