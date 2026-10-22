using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
namespace Bai06d_QLThuVien_3Lop.DAL;

// DAL - Data Access Layer: chịu trách nhiệm duy nhất về truy cập SQL Server.
internal sealed class Database
{
    internal string ConnectionString
    {
        get {
            string? setting=Environment.GetEnvironmentVariable("TUAN06_THUVIENDB");
            if(!string.IsNullOrWhiteSpace(setting))return setting;
            string path=Path.Combine(AppContext.BaseDirectory,"dbsettings.json");
            using var doc=JsonDocument.Parse(File.ReadAllText(path));
            return doc.RootElement.GetProperty("ConnectionString").GetString()
                ?? throw new InvalidOperationException("Thiếu ConnectionString");
        }
    }
    internal DataTable Execute(string sql,params SqlParameter[] parameters)
    {
        using var conn=new SqlConnection(ConnectionString);
        using var command=new SqlCommand(sql,conn);
        command.Parameters.AddRange(parameters);
        using var adapter=new SqlDataAdapter(command);
        var table=new DataTable();adapter.Fill(table);return table;
    }
    internal int ExecuteNonQuery(string sql,params SqlParameter[] parameters)
    {
        using var conn=new SqlConnection(ConnectionString);
        using var command=new SqlCommand(sql,conn);
        command.Parameters.AddRange(parameters);
        conn.Open();return command.ExecuteNonQuery();
    }
    // Bài tập 5: cập nhật dữ liệu trực tiếp trên DataGridView qua SqlDataAdapter.
    internal int Update(string tableName,DataTable data)
    {
        // Tên bảng chỉ từ danh sách bảng do mã nguồn xác định; không lấy từ textbox.
        using var adapter=new SqlDataAdapter($"SELECT * FROM dbo.[{tableName}]",ConnectionString);
        adapter.MissingSchemaAction=MissingSchemaAction.AddWithKey;
        using var builder=new SqlCommandBuilder(adapter) { QuotePrefix="[",QuoteSuffix="]" };
        adapter.InsertCommand=builder.GetInsertCommand();
        adapter.UpdateCommand=builder.GetUpdateCommand();
        adapter.DeleteCommand=builder.GetDeleteCommand();
        return adapter.Update(data);
    }
}
