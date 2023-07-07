using CMPS339_PROJECT.Models;

namespace CMPS339_PROJECT.Services.Interfaces
{
    public interface IAmusementParkService
    {
        Task<List<Parks>> GetAllAsync();
        Task<Parks?> GetByIdAsync(int id);
        Task<ParksGetDto?> InsertAsync(ParksCreateDto dto);
        Task UpdateAsync(int id, ParksCreateUpdateDto dto);
    }
}