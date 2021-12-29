namespace WebApi.Extensions.CitizensFilters;

using Domain.Entities;
using Domain.Enums;

public static class CitizensSexFilterExtension
{
    public static List<Citizen> UseSexFilter(this List<Citizen> citizens, CitizenSex? sex)
    {
        if (sex != null)
            citizens.RemoveAll(c => c.Sex != sex);

        return citizens;
    }
}