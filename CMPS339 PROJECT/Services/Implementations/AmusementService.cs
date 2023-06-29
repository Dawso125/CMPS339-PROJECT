using CMPS339_PROJECT.Models;
using CMPS339_PROJECT.Services.Interfaces;
using Dapper;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CMPS339_PROJECT.Services.Implementations
{
    public class AmusementService : IAmusementParkService
    {
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
    }
}
