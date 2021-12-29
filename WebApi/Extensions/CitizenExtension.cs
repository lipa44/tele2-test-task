namespace WebApi.Extensions;

using Domain.Entities;
using Dto;

public static class CitizenExtension
{
    public static CitizenDto ToDto(this Citizen citizen)
        => new (
            citizen.Id,
            citizen.Name,
            citizen.Surname,
            citizen.Sex.ToString());

    public static CitizenFullDto ToFullDto(this Citizen citizen)
        => new (
            citizen.Id,
            citizen.Name,
            citizen.Surname,
            citizen.Age,
            citizen.Sex.ToString());
}