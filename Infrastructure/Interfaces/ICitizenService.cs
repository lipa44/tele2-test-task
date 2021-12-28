namespace Infrastructure.Interfaces;

using Domain.Entities;

public interface ICitizenService
{
    public Task<ICollection<Citizen>> GetAllCitizensAsync();
    public Task<Citizen> GetCitizenByIdAsync(Guid citizenId);
    public Task<Citizen?> FindCitizenByIdAsync(Guid citizenId);
    public Task<Citizen> RegisterCitizen(Citizen citizen);
}