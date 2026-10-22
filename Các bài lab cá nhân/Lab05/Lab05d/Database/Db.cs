using Microsoft.Data.SqlClient;

namespace Lab05d.Database;

public static class Db
{
    public static string ConnectionString =
        @"Server=DUCK\DUCK;Database=QLSinhVien;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection CreateConnection()
        => new(ConnectionString);
}