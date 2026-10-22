using Microsoft.Data.SqlClient;

namespace Lab06a.Database;

public static class Db
{
    public static string ConnectionString =
        @"Server=DUCK\DUCK;Database=Lab06_QLSinhVien;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection CreateConnection()
        => new SqlConnection(ConnectionString);
}