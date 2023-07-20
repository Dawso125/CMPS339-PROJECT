using CMPS339_PROJECT.Models;
using CMPS339_PROJECT.Services.Interfaces;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CMPS339_PROJECT.Services.Implementations
{
    public class AttractionsService : IAttractionsService
    {

        private readonly ILogger<AttractionsService> _logger;
        private readonly IMemoryCache _cache;

        public AttractionsService(ILogger<AttractionsService> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<List<Attraction>> GetAllAsync()
        {
            List<Attraction> attractions = new();
            using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
            {
                connection.Open();

                var attractionData = await connection.QueryAsync<Attraction>("SELECT * FROM Attractions");

                attractions = attractionData.ToList();
            }
            return attractions;
        }

        public async Task<Attraction?> GetByIdAsync(int id)
        {
            List<Attraction> attractions = new();

            using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
            {
                connection.Open();

                var attractionData = await connection.QueryAsync<Attraction>("SELECT * FROM Attractions WHERE Id = @Id", new { Id = id });
                attractions = attractionData.ToList();

            }
            return attractions.FirstOrDefault();

        }

        public async Task<AttractionDto?> InsertAsync(AttractionCreateDto dto)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
                {
                    connection.Open();

                    string sqlInsertQuery = "INSERT INTO ATTRACTIONS (ParkId) OUTPUT INSERTED.Id, INSERTED.ParkId VALUES (@ParkId)";
                    string sqlFetchParkQuery = "SELECT Name FROM Parks WHERE Id = @ParkId";

                    
                    using (var multi = await connection.QueryMultipleAsync($"{sqlInsertQuery}; {sqlFetchParkQuery}", new { ParkId = dto.ParkId }))
                    {
                        
                        Attraction newAttraction = await multi.ReadSingleAsync<Attraction>();

                        string parkName = await multi.ReadSingleOrDefaultAsync<string>();

                        return new AttractionDto
                        {
                            Id = newAttraction.Id,
                            ParkId = newAttraction.ParkId,
                            Park = parkName
                        };
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occurred DTO Value ParkId: {PARKID}", dto.ParkId);
                return null;
            }
        }


    }
}
