namespace Infrastructure.Services;

using DataAccess;
using Domain.Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class CitizensService : ICitizenService
{
    private readonly ApplicationDbContext _dbContext;

    public CitizensService(ApplicationDbContext context)
        => _dbContext = context;

    public async Task<List<Citizen>> GetCitizensAsync()
        => await _dbContext.Citizens.ToListAsync();

    public async Task<Citizen?> FindCitizenByIdAsync(Guid citizenId)
        => await _dbContext.Citizens.SingleOrDefaultAsync(citizen => citizen.Id == citizenId);
}