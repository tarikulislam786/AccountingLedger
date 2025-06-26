using AccountingLedger.Domain.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AccountingLedger.Infrastructure.Services
{
    public class JournalRepository
    {
        private readonly IConfiguration _configuration;

        public JournalRepository(IConfiguration configuration) =>
            _configuration = configuration;

        #region ========== Create JournalEntry ==========
        public async Task<int> CreateJournalEntryAsync(
    DateTime date,
    string description,
    List<JournalLineDto> lines)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("sp_CreateJournalEntry", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Date", date);
            cmd.Parameters.AddWithValue("@Description", description);

            // Create table-valued parameter
            var linesTable = new DataTable();
            linesTable.Columns.Add("AccountId", typeof(int));
            linesTable.Columns.Add("Debit", typeof(decimal));
            linesTable.Columns.Add("Credit", typeof(decimal));

            foreach (var line in lines)
            {
                linesTable.Rows.Add(line.AccountId, line.Debit, line.Credit);
            }

            var linesParam = cmd.Parameters.AddWithValue("@Lines", linesTable);
            linesParam.SqlDbType = SqlDbType.Structured;
            linesParam.TypeName = "JournalLineType"; // Must match SQL Table Type

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }
        #endregion


        #region ========== Get Journal Entries ==========
        public async Task<List<JournalEntryDto>> GetJournalEntriesAsync()
        {
            var entryMap = new Dictionary<int, JournalEntryDto>();

            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("sp_GetJournalEntries", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var journalId = reader.GetInt32(0);
                var date = reader.GetDateTime(1);
                var description = reader.GetString(2);
                var accountId = reader.GetInt32(3);
                var accountName = reader.GetString(4);
                var debit = reader.GetDecimal(5);
                var credit = reader.GetDecimal(6);

                // Create the line
                var line = new JournalLineDto
                {
                    AccountId = accountId,
                    AccountName = accountName,
                    Debit = debit,
                    Credit = credit
                };

                // If entry already exists, add the line
                if (entryMap.ContainsKey(journalId))
                {
                    entryMap[journalId].Lines.Add(line);
                }
                else
                {
                    // Create new entry
                    entryMap[journalId] = new JournalEntryDto
                    {
                        Id = journalId,
                        Date = date,
                        Description = description,
                        Lines = new List<JournalLineDto> { line }
                    };
                }
            }

            return entryMap.Values.ToList();
        }
        #endregion


        #region ========== Get Trial Balance ==========
        public async Task<List<TrialBalanceDto>> GetTrialBalanceAsync()
        {
            var results = new List<TrialBalanceDto>();

            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("sp_GetTrialBalance", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new TrialBalanceDto
                {
                    AccountId = reader.GetInt32(0),
                    AccountName = reader.GetString(1),
                    AccountType = reader.GetString(2),
                    Balance = reader.IsDBNull(3) ? 0m : reader.GetDecimal(3) // ✅ Fallback to 0
                });
            }

            return results;
        }
        #endregion
    }
}
