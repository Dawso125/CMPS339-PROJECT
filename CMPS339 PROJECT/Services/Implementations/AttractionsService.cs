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

    }
}
