IF DB_ID(N'QLSinhVien') IS NULL
    CREATE DATABASE QLSinhVien;
GO
USE QLSinhVien;
GO

IF OBJECT_ID('Diem') IS NOT NULL DROP TABLE Diem;
IF OBJECT_ID('SinhVien') IS NOT NULL DROP TABLE SinhVien;
IF OBJECT_ID('MonHoc') IS NOT NULL DROP TABLE MonHoc;
IF OBJECT_ID('Lop') IS NOT NULL DROP TABLE Lop;
IF OBJECT_ID('Khoa') IS NOT NULL DROP TABLE Khoa;
GO

CREATE TABLE Khoa(
    MaKhoa NVARCHAR(20) PRIMARY KEY,
    TenKhoa NVARCHAR(100) NOT NULL
);
CREATE TABLE Lop(
    MaLop NVARCHAR(20) PRIMARY KEY,
    TenLop NVARCHAR(100) NOT NULL,
    MaKhoa NVARCHAR(20) NOT NULL REFERENCES Khoa(MaKhoa)
);
CREATE TABLE SinhVien(
    MaSinhVien NVARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE NOT NULL,
    MaLop NVARCHAR(20) NOT NULL REFERENCES Lop(MaLop)
);
CREATE TABLE MonHoc(
    MaMonHoc NVARCHAR(20) PRIMARY KEY,
    TenMonHoc NVARCHAR(100) NOT NULL
);
CREATE TABLE Diem(
    MaSinhVien NVARCHAR(20) NOT NULL REFERENCES SinhVien(MaSinhVien),
    MaMonHoc NVARCHAR(20) NOT NULL REFERENCES MonHoc(MaMonHoc),
    Diem FLOAT NOT NULL,
    CONSTRAINT PK_Diem PRIMARY KEY(MaSinhVien,MaMonHoc)
);
GO

INSERT Khoa VALUES
(N'CNTT',N'Công nghệ thông tin'),
(N'KT',N'Kế toán'),
(N'QTKD',N'Quản trị kinh doanh'),
(N'NN',N'Ngoại ngữ'),
(N'DL',N'Du lịch');

INSERT Lop VALUES
(N'05DHTH1',N'Công nghệ phần mềm khóa 05',N'CNTT'),
(N'05DHTH2',N'Kế toán ngân hàng khóa 05',N'KT'),
(N'05DHTH3',N'Lập trình mạng khóa 05',N'CNTT'),
(N'06DHTH1',N'Hòa phân tích khóa 06',N'CNTT'),
(N'06DHTH2',N'Thuế doanh nghiệp khóa 06',N'KT');

INSERT SinhVien VALUES
(N'SV001',N'Trần Thanh Tùng','1993-10-12',N'05DHTH1'),
(N'SV002',N'Trần Thanh Trinh','1993-10-12',N'05DHTH1'),
(N'SV003',N'Nguyễn Thanh Bình','1993-04-02',N'05DHTH2'),
(N'SV004',N'Nguyễn Thanh Bình','1993-04-02',N'05DHTH2'),
(N'SV005',N'Nguyễn Đạt Nhân','1992-03-15',N'05DHTH3');

INSERT MonHoc VALUES
(N'1',N'Toán cao cấp'),
(N'2',N'Mac - Lênin'),
(N'3',N'Tin học đại cương'),
(N'4',N'Cơ sở dữ liệu'),
(N'5',N'Lập trình C#');

INSERT Diem VALUES
(N'SV001',N'1',4),
(N'SV001',N'2',7),
(N'SV002',N'1',6),
(N'SV003',N'2',6),
(N'SV004',N'3',5);
GO
