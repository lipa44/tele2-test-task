namespace WebApi.Dto;

public record CitizenDto(
    Guid Id,
    string Name,
    string Surname,
    string Sex);