# Lab06 - Lập trình tương tác cơ sở dữ liệu

## Nội dung
- Lab06a: Tương tác CSDL (1): SqlConnection + SqlCommand, CRUD trên QLSinhVien.
- Lab06b: Tương tác CSDL (2): SqlDataReader, ComboBox khóa ngoại, kiểm tra khóa, ListView và bài QLCuaHang.

## Cài SqlClient
Chạy trong từng project:
```powershell
dotnet add .\Lab06a\Lab06a.csproj package Microsoft.Data.SqlClient
dotnet add .\Lab06b\Lab06b.csproj package Microsoft.Data.SqlClient
```

## Database
Mở SSMS và chạy:
1. setup_QLSinhVien.sql
2. setup_QLCuaHang.sql

Nếu không dùng .\SQLEXPRESS, sửa chuỗi kết nối:
- Lab06a/Database/Db.cs
- Lab06b/Database/Db.cs

## Build
```powershell
cd "D:\CSharp\Các bài lab cá nhân\Lab06"
dotnet restore
dotnet build Lab06.slnx
```

## Run
```powershell
dotnet run --project .\Lab06a\Lab06a.csproj
dotnet run --project .\Lab06b\Lab06b.csproj
```

## Ghi chú về bài bán hàng
Tài liệu ghi công thức tồn:
SLTon = SLDau + SLNhap - SLBan.
Vì vậy khi bán, source này cập nhật SLBan = SLBan + SoLuong.
Dòng PDF ghi "SLBan = SLBan - số lượng" có vẻ không phù hợp với công thức tồn; đây là lựa chọn hiện thực có chủ đích.
