using CMPS339_PROJECT.Models;
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
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IMemoryCache _cache;

        public UserService(ILogger<UserService> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<List<Users>> GetAllAsync()
        {
            List<Users> users = new();

            using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
            {
                connection.Open();

                var userData = await connection.QueryAsync<Users>("SELECT * FROM Users");

                users = userData.ToList();
            }

            return users;
        }

        public async Task<Users?> GetByIdAsync(int id)
        {
            List<Users> users = new();

            using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
            {
                connection.Open();
                var userData = await connection.QueryAsync<Users>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });

                users = userData.ToList();
            }

            return users.FirstOrDefault();
        }

        public async Task<UsersGetDto?> InsertAsync(UsersCreateDto dto)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
                {
                    connection.Open();
                    IEnumerable<Users> newUser = await connection.QueryAsync<Users>("INSERT INTO USERS OUTPUT INSERTED.* VALUES (@Username, @Password, @IsActive)", new { Username = dto.Username, Password = dto.Passwords, IsActive = dto.IsActive });
                    return newUser.Select(x => new UsersGetDto
                    {
                        Id = x.Id,
                        Username = x.Username,
                        Passwords = x.Passwords,
                        IsActive = x.IsActive,

                    }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occurred. DTO Value Username: {USERNAME} AT: {TIME}", dto.Username, DateTime.Now.ToString());
                _logger.LogError(e, "An error has occurred. DTO Value Password: {PASSWORD} AT: {TIME}", dto.Passwords, DateTime.Now.ToString());
                _logger.LogError(e, "An error has occurred. DTO Value IsActive: {ISACTIVE} AT: {TIME}", dto.IsActive, DateTime.Now.ToString());
                return null;
            }
        }

        public async Task<UsersGetDto?> UpdateAsync(int id, UsersCreateUpdateDto dto)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
                {
                    connection.Open();
                    IEnumerable<Users> updatedUsers = await connection.QueryAsync<Users>("UPDATE Users SET Username = @Username WHERE Id = @Id; UPDATE Users SET Passwords = @Passwords WHERE Id = @Id; UPDATE Users SET IsActive = @IsActive WHERE Id = @Id; SELECT * FROM Users WHERE Id = @Id", new { Username = dto.Username, Passwords = dto.Passwords, IsActive = dto.IsActive, Id = id });
                    return updatedUsers.Select(x => new UsersGetDto
                    {
                        Id = x.Id,
                        Username = x.Username,
                        Passwords = x.Passwords,
                        IsActive = x.IsActive,

                    }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occurred. DTO Value Username {USERNAME} AT: {TIME}", dto.Username, DateTime.Now.ToString());
                _logger.LogError(e, "An error has occurred. DTO Value Password: {PASSWORDS} AT: {TIME}", dto.Passwords, DateTime.Now.ToString());
                _logger.LogError(e, "An error has occurred. DTO Value IsActive: {ISACTIVE} AT: {TIME}", dto.IsActive, DateTime.Now.ToString());
                return null;
            }
        }

        public async Task<UsersGetDto?> DeleteByIdAsync(int id)
        {
            try
            {
                List<Users> Users = new();
                using (IDbConnection connection = new SqlConnection(ConnectionService.ConnectionString))
                {
                    connection.Open();
                    var User = await GetByIdAsync(id);
                    if (User != null)
                    {
                        var deleteQuery = "DELETE FROM Users WHERE Id = @Id";
                        await connection.QueryAsync(deleteQuery, new { Id = id }); // Execute the delete query

                        return new UsersGetDto // Return the deleted park information
                        {
                            // Map relevant properties from the deleted park to ParksGetDto
                            Id = User.Id,
                            Username = User.Username,
                            Passwords = User.Passwords,
                            IsActive = User.IsActive,
                            // Add other properties as needed
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            catch (Exception e)
            {
                _logger.LogError(e, "An error has occurred. DTO Value Username {USERNAME} AT: {TIME}");
                _logger.LogError(e, "An error has occurred. DTO Value Password: {PASSWORD} AT: {TIME}");
                _logger.LogError(e, "An error has occurred. DTO Value IsActive: {ISACTIVE} AT: {TIME}");
                return null;

            }
        }
    }
}
