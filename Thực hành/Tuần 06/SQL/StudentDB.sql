-- Bài thực hành 06c - StudentDB (SQL Server).
IF DB_ID(N'StudentDB') IS NULL CREATE DATABASE StudentDB;
GO
USE StudentDB;
GO
IF OBJECT_ID(N'dbo.Khoa',N'U') IS NULL
CREATE TABLE dbo.Khoa (
    MaKhoa nvarchar(10) NOT NULL PRIMARY KEY,
    TenKhoa nvarchar(100) NOT NULL
);
GO
IF OBJECT_ID(N'dbo.Mon',N'U') IS NULL
CREATE TABLE dbo.Mon (
    MaMH nvarchar(10) NOT NULL PRIMARY KEY,
    TenMH nvarchar(100) NOT NULL,
    SoTiet int NOT NULL CHECK(SoTiet > 0)
);
GO
IF OBJECT_ID(N'dbo.SinhVien',N'U') IS NULL
CREATE TABLE dbo.SinhVien (
    MaSo nvarchar(12) NOT NULL PRIMARY KEY,
    HoTen nvarchar(100) NOT NULL,
    NgaySinh date NULL,
    GioiTinh bit NOT NULL DEFAULT (1),
    DiaChi nvarchar(200) NULL,
    DienThoai nvarchar(20) NULL,
    Khoa nvarchar(10) NOT NULL REFERENCES dbo.Khoa(MaKhoa)
);
GO
IF OBJECT_ID(N'dbo.KetQua',N'U') IS NULL
CREATE TABLE dbo.KetQua (
    MaSo nvarchar(12) NOT NULL REFERENCES dbo.SinhVien(MaSo),
    MaMH nvarchar(10) NOT NULL REFERENCES dbo.Mon(MaMH),
    Diem decimal(4,2) NOT NULL CHECK(Diem BETWEEN 0 AND 10),
    CONSTRAINT PK_KetQua PRIMARY KEY(MaSo,MaMH)
);
GO
IF NOT EXISTS(SELECT 1 FROM dbo.Khoa)
INSERT INTO dbo.Khoa VALUES
(N'CNTT',N'Công nghệ thông tin'), (N'KT',N'Kế toán'), (N'QTKD',N'Quản trị kinh doanh');
IF NOT EXISTS(SELECT 1 FROM dbo.Mon)
INSERT INTO dbo.Mon VALUES
(N'MH01',N'Lập trình C#',45),(N'MH02',N'Cơ sở dữ liệu',45),
(N'MH03',N'Mạng máy tính',30),(N'MH04',N'Cấu trúc dữ liệu',45);
IF NOT EXISTS(SELECT 1 FROM dbo.SinhVien)
INSERT INTO dbo.SinhVien(MaSo,HoTen,NgaySinh,GioiTinh,DiaChi,DienThoai,Khoa) VALUES
(N'SV001',N'Nguyễn Minh An','2004-05-08',1,N'TP.HCM',N'0900000001',N'CNTT'),
(N'SV002',N'Trần Thu Hà','2004-09-10',0,N'Đồng Nai',N'0900000002',N'CNTT'),
(N'SV003',N'Lê Hoàng Nam','2003-02-14',1,N'Tây Ninh',N'0900000003',N'KT'),
(N'SV004',N'Phạm Ngọc Mai','2005-12-01',0,N'Bình Dương',N'0900000004',N'QTKD');
IF NOT EXISTS(SELECT 1 FROM dbo.KetQua)
INSERT INTO dbo.KetQua VALUES
(N'SV001',N'MH01',8.50),(N'SV001',N'MH02',7.75),(N'SV002',N'MH01',9.00),
(N'SV003',N'MH02',6.50),(N'SV004',N'MH03',8.00);
GO
