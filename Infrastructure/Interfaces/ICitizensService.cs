namespace Infrastructure.Interfaces;

using Domain.Entities;

public interface ICitizenService
{
    public Task<List<Citizen>> GetCitizensAsync();
    public Task<Citizen> GetCitizenByIdAsync(Guid citizenId);
}