using CMPS339_PROJECT.Models;

namespace CMPS339_PROJECT.Services.Interfaces
{
    public class IUserService
    { 
            Task<List<Users>> GetAllAsync();
            Task<Users?> GetByIdAsync(int id);
            Task<UsersGetDto?> InsertAsync(UsersCreateDto dto);
            Task<UsersGetDto?> UpdateAsync(int id, UsersCreateUpdateDto dto);
            Task<UsersGetDto?> DeleteByIdAsync(int id);
        }
    }
}
}
