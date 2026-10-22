# Lab05 - Lập trình WinForms nâng cao (đầy đủ)

## Lab05c
- Mẫu 1: ListBox chuyển phần tử.
- Mẫu 2: ComboBox dân tộc.
- Mẫu 3: TreeView phòng ban - nhân viên.
- Tại lớp 1: ComboBox số + ListBox ước số.
- Tại lớp 2: Quản lý sinh viên bằng TreeView.
- Nâng cao: Xử lý chuỗi / 50 tên ngẫu nhiên.
- BTVN 1: Từ điển Anh - Việt / Việt - Anh.
- BTVN 2: Xử lý ListBox số.
- BTVN 3: Danh bạ TreeView A-Z.

## Lab05d
- Mẫu 1: Timer hiển thị thời gian.
- Mẫu 2: ListView thêm/xóa/sửa.
- Mẫu 3: OpenFileDialog / SaveFileDialog.
- Tại lớp 1: ListView sinh viên + ContextMenuStrip.
- Tại lớp 2: Quản lý tài khoản.
- Nâng cao: Quản lý học viên bằng MenuStrip.
- BTVN 1: Đếm ngược.
- BTVN 2: MainForm MenuStrip - đã dùng chính MainForm để mở toàn bộ bài.
- Bài 7 ADO.NET:
  - Mẫu: Quản lý Khoa.
  - Tại lớp: Quản lý Môn học.
  - Nâng cao: Quản lý Sinh viên, Quản lý Điểm.
  - BTVN: Quản lý Lớp.

## Cài gói SQL Client
```powershell
cd Lab05d
dotnet restore
```
Nếu PackageReference có vấn đề:
```powershell
dotnet add package Microsoft.Data.SqlClient
```

## Database
Chạy `setup_QLSinhVien.sql` trong SSMS, sau đó sửa chuỗi kết nối trong:
`Lab05d/Database/Db.cs`

## Build
```powershell
cd "D:\CSharp\Các bài lab cá nhân\Lab05"
dotnet restore
dotnet build Lab05.slnx
```

## Chạy
```powershell
dotnet run --project .\Lab05c\Lab05c.csproj
dotnet run --project .\Lab05d\Lab05d.csproj
```
