using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
namespace Bai06c_QuanLySinhVien;
internal static class SqlDatabase
{
    internal static string ConnectionString
    {
        get
        {
            string? environment = Environment.GetEnvironmentVariable("TUAN06_STUDENTDB");
            if (!string.IsNullOrWhiteSpace(environment)) return environment;
            string path = Path.Combine(AppContext.BaseDirectory, "dbsettings.json");
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            return doc.RootElement.GetProperty("ConnectionString").GetString()
                   ?? throw new InvalidOperationException("Thiếu ConnectionString trong dbsettings.json");
        }
    }

    internal static DataTable Query(string sql, params SqlParameter[] parameters)
    {
        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddRange(parameters);
        using var adapter = new SqlDataAdapter(command);
        var table = new DataTable();
        adapter.Fill(table);
        return table;
    }
}
