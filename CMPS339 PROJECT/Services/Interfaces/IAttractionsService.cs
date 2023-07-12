using CMPS339_PROJECT.Models;

namespace CMPS339_PROJECT.Services.Interfaces
{
    public interface IAttractionsService
    {
        Task<List<Attraction>> GetAllAsync();
        Task<Attraction?> GetByIdAsync(int id);

    }
}
