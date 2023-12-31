﻿using CMPS339_PROJECT.Models;
using CMPS339_PROJECT.Services.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occurred. DTO Value Name: {NAME} AT: {TIME}", dto.Name, DateTime.Now.ToString());
                return null;
            }
        }

        public async Task<ParksGetDto?> UpdateAsync(int id, ParksCreateUpdateDto dto)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
                {
                    connection.Open();
                    IEnumerable<Parks> updatedPark = await connection.QueryAsync<Parks>("UPDATE Parks SET Name = @Name WHERE Id = @Id; SELECT * FROM Parks WHERE Id = @Id", new { Name = dto.Name, Id = id });
                    return updatedPark.Select(x => new ParksGetDto
                    {
                        Id = x.Id,
                        Name = x.Name,

                    }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occurred. DTO Value Name: {NAME} AT: {TIME}", dto.Name, DateTime.Now.ToString());
                return null;
            }
        }

        public async Task<ParksGetDto?> DeleteByIdAsync(int id)
        {
            try
            {
                List<Parks> parks = new();
                using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
                {
                    connection.Open();
                    var park = await GetByIdAsync(id);
                    if (park != null)
                    {
                        var deleteQuery = "DELETE FROM Parks WHERE Id = @Id";
                        await connection.QueryAsync(deleteQuery, new { Id = id }); // Execute the delete query

                        return new ParksGetDto // Return the deleted park information
                        {
                            // Map relevant properties from the deleted park to ParksGetDto
                            Id = park.Id,
                            Name = park.Name,
                            // Add other properties as needed
                        };
                    } else
                    {
                        return null;
                    }
                }
            }

            catch (Exception e)
            {
                _logger.LogError(e, "An error has occured. DTO Value Name: {NAME} AT: {TIME}");
                return null;

            }
        }
    }
}
