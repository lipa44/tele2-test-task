namespace DataAccess.Extensions;

using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Citizen>().HasData(
            new Citizen(
                Guid.NewGuid(),
                "Misha",
                "Libchenko",
                19,
                CitizenSex.Male),
            new Citizen(
                Guid.NewGuid(),
                "Stan",
                "Smith",
                27,
                CitizenSex.Male),
            new Citizen(
                Guid.NewGuid(),
                "Jack",
                "Anderson",
                44,
                CitizenSex.Male),
            new Citizen(
                Guid.NewGuid(),
                "Olga",
                "Popova",
                24,
                CitizenSex.Female)
        );
    }
}