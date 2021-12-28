namespace WebUI.Extensions;

using Domain.Entities;
using Dto;

public static class CitizenExtension
{
    public static CitizenDto ToDto(this Citizen citizen)
        => new CitizenDto(
            citizen.Id,
            citizen.Name,
            citizen.Surname,
            citizen.Gender.ToString());

    public static CitizenFullDto ToFullDto(this Citizen citizen)
        => new CitizenFullDto(
            citizen.Id,
            citizen.Name,
            citizen.Surname,
            citizen.Age,
            citizen.Gender.ToString());
}