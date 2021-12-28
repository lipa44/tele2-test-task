namespace Infrastructure.Services;

using DataAccess;
using Domain.Entities;
using Domain.Exceptions;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class CitizenService : ICitizenService
{
    private readonly ApplicationDbContext _dbContext;

    public CitizenService(ApplicationDbContext context) => _dbContext = context;

    public async Task<ICollection<Citizen>> GetAllCitizensAsync()
        => await _dbContext.Citizens.ToListAsync();

    public async Task<Citizen?> FindCitizenByIdAsync(Guid citizenId)
        => await _dbContext.Citizens.SingleOrDefaultAsync(citizen => citizen.Id == citizenId);

    public async Task<Citizen> RegisterCitizen(Citizen citizen)
    {
        if (await _dbContext.Citizens.FindAsync(citizen.Id) != null)
            throw new Tele2Exception($"Citizen with Id {citizen.Id} already exists");
        
        await _dbContext.Citizens.AddAsync(citizen);
        await _dbContext.SaveChangesAsync();

        return citizen;
    }

    public async Task<Citizen> GetCitizenByIdAsync(Guid citizenId)
        => await _dbContext.Citizens.SingleOrDefaultAsync(citizen => citizen.Id == citizenId)
           ?? throw new Tele2Exception($"Citizen with Id {citizenId} doesn't exist");
}