using CMPS339_PROJECT.Models;
using CMPS339_PROJECT.Services.Interfaces;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CMPS339_PROJECT.Services.Implementations
{
    public class AmusementService : IAmusementParkService
    {
        private readonly ILogger<AmusementService> _logger;
        private readonly IMemoryCache _cache;
        public AmusementService(ILogger<AmusementService> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<List<Parks>> GetAllAsync()
        {
            List<Parks> parks = new();

            using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
            {
                connection.Open();

                var parkData = await connection.QueryAsync<Parks>("SELECT * FROM Parks");

                parks = parkData.ToList();
            }

            return parks;
        }

        public async Task<Parks?> GetByIdAsync(int id)
        {
            List<Parks> parks = new();

            using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
            {
                connection.Open();
                var parkData = await connection.QueryAsync<Parks>("SELECT * FROM Parks WHERE Id = @Id", new { Id = id });

                parks = parkData.ToList();
            }

            return parks.FirstOrDefault();
        }

        public async Task<ParksGetDto?> InsertAsync(ParksCreateDto dto)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
                {
                    connection.Open();
                    IEnumerable<Parks> newPark = await connection.QueryAsync<Parks>("INSERT INTO PARKS OUTPUT INSERTED.* VALUES (@Name)", new { Name = dto.Name });
                    return newPark.Select(x => new ParksGetDto
                    {
                        Id = x.Id,
                        Name = x.Name,

                    }).FirstOrDefault();

                }
            } catch (Exception e) 
            { 
                _logger.LogError(e, "An error has occured. DTO Value Name: {NAME} AT: {TIME}", dto.Name, DateTime.Now.ToString());
                return null;
            
            }
        }

        public async Task<ParksDeleteDto?> DeleteByIdAsync(int id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
                {
                    connection.Open();
                    IEnumerable<Parks> newPark = await connection.QueryAsync<Parks>("DELETE * FROM Parks WHERE Id = @Id", new { Id = id });
                    return newPark.Select(x => new ParksDeleteDto
                    {
                        Id = x.Id,
                        Name = x.Name,

                    }).FirstOrDefault();

                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occured. DTO Value Name: {NAME} AT: {TIME}", dto.Name, DateTime.Now.ToString());
                return null;

            }
        }
    }
}
