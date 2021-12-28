namespace WebUI.Dto;

public record CitizenFullDto(
    Guid Id,
    string Name,
    string Surname,
    uint Age,
    string Gender);