using Microsoft.Data.SqlClient;

namespace Lab06b.Database;

public static class Db
{
    public static string QLSinhVien =
        @"Server=DUCK\DUCK;Database=Lab06_QLSinhVien;Trusted_Connection=True;TrustServerCertificate=True;";

    public static string QLCuaHang =
        @"Server=DUCK\DUCK;Database=Lab06_QLCuaHang;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection SinhVienConn()
        => new SqlConnection(QLSinhVien);

    public static SqlConnection CuaHangConn()
        => new SqlConnection(QLCuaHang);
}