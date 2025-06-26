using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AccountingLedger.Infrastructure.Services
{
    public class AccountRepository
    {
        private readonly IConfiguration _configuration;

        public AccountRepository(IConfiguration configuration) => _configuration = configuration;

        #region ========== Create Account ==========
        public async Task<int> CreateAccountAsync(string name, string type)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("sp_CreateAccount", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Type", type);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }
        #endregion
        #region ========== Get Accounts ==========
        public async Task<List<(int Id, string Name, string Type)>> GetAccountsAsync()
        {
            var results = new List<(int, string, string)>();
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("sp_GetAccounts", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add((reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            }
            return results;
        }
        #endregion
    }
}
