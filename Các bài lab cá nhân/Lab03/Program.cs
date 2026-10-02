using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiThucHanhLINQ;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        // Ví dụ: dotnet run -- 2.1; không truyền tham số thì chạy tất cả.
        Chay(args.FirstOrDefault() ?? "all");
    }

    static void Chay(string bai)
    {
        switch (bai)
        {
            case "2.1": Bai21(); break;
            case "2.2": Bai22(); break;
            case "3.1": Bai31(); break;
            case "3.2": Bai32(); break;
            case "4.1": Bai41(); break;
            case "5.1": Bai51(); break;
            case "5.2": Bai52(); break;
            case "6.1": Bai61(); break;
            case "6.2": Bai62(); break;
            case "all":
                Bai21(); Bai22(); Bai31(); Bai32(); Bai41();
                Bai51(); Bai52(); Bai61(); Bai62();
                break;
            default:
                Console.WriteLine("Chọn: 2.1, 2.2, 3.1, 3.2, 4.1, 5.1, 5.2, 6.1, 6.2 hoặc all.");
                break;
        }
    }

    // Các hàm In... chỉ xuất kết quả, không thay LINQ bằng vòng lặp xử lý.
    static void TieuDe(string nhan) => Console.WriteLine("\n=== " + nhan + " ===");

    static string NhanHe(string maHe) =>
        string.IsNullOrWhiteSpace(maHe) ? "(chưa khai báo hệ)" : maHe;

    static void InDay<T>(string nhan, IEnumerable<T> duLieu)
    {
        TieuDe(nhan);
        Console.WriteLine(string.Join(", ", duLieu));
    }

    static void InDong<T>(string nhan, IEnumerable<T> duLieu)
    {
        TieuDe(nhan);
        foreach (var dong in duLieu)
            Console.WriteLine(dong);
    }

    static void InMon(string nhan, IEnumerable<MonHoc> duLieu)
    {
        TieuDe(nhan);
        foreach (var m in duLieu)
            Console.WriteLine($"{m.MaMon,-7} | {m.TenMon} | {NhanHe(m.He)} | {m.SoTiet} tiết");
    }

    static void InNhom<T>(string nhan, IEnumerable<IGrouping<T, MonHoc>> cacNhom)
    {
        TieuDe(nhan);
        foreach (var nhom in cacNhom)
        {
            string khoa = $"{nhom.Key}";
            Console.WriteLine($"Nhóm: {NhanHe(khoa)} ({nhom.Count()} môn)");
            foreach (var m in nhom)
                Console.WriteLine($"  {m.MaMon,-7} | {m.TenMon} | {NhanHe(m.He)} | {m.SoTiet} tiết");
        }
    }

    static void Bai21()
    {
        int[] mangSo = { 50, 42, 16, 3, 9, 8, 12, 7, 24, 0 };

        // a: && nghĩa là đồng thời thỏa cả hai điều kiện. Số 0 cũng thỏa.
        var aQuery = from x in mangSo
                     where x % 4 == 0 && x % 3 == 0
                     select x;
        var aMethod = mangSo.Where(x => x % 4 == 0 && x % 3 == 0);
        InDay("2.1a - Query: chia hết cho 4 và 3", aQuery);
        InDay("2.1a - Method", aMethod);

        var bQuery = from x in mangSo where x <= 3 select x;
        var bMethod = mangSo.Where(x => x <= 3);
        InDay("2.1b - Query: nhỏ hơn hoặc bằng 3", bQuery);
        InDay("2.1b - Method", bMethod);

        // c: Select biến đổi từng phần tử, vẫn giữ nguyên số lượng phần tử.
        // Biểu thức điều kiện: điều_kiện ? giá_trị_khi_đúng : giá_trị_khi_sai.
        var cQuery = from x in mangSo select x % 2 == 0 ? x / 2 : x;
        var cMethod = mangSo.Select(x => x % 2 == 0 ? x / 2 : x);
        InDay("2.1c - Query: chẵn chia đôi, lẻ giữ nguyên", cQuery);
        InDay("2.1c - Method", cMethod);
    }

    static void Bai22()
    {
        string[] mangChuoi = { "đầu", "lòng", "hai", "ả", "tố", "nga",
            "Thúy", "Kiều", "là", "chị", "em", "là", "Thúy", "Vân" };

        // Đề yêu cầu sắp theo ký tự đầu tiên, nên dùng s[0], không phải cả chuỗi.
        var aQuery = from s in mangChuoi where s.Length == 4 orderby s[0] select s;
        var aMethod = mangChuoi.Where(s => s.Length == 4).OrderBy(s => s[0]);
        InDay("2.2a - Query: 4 ký tự, tăng theo ký tự đầu", aQuery);
        InDay("2.2a - Method", aMethod);

        var bQuery = from s in mangChuoi select $"{s.ToLowerInvariant()} - {s.ToUpperInvariant()}";
        var bMethod = mangChuoi.Select(s => $"{s.ToLowerInvariant()} - {s.ToUpperInvariant()}");
        InDay("2.2b - Query: chữ thường - CHỮ HOA", bQuery);
        InDay("2.2b - Method", bMethod);

        // 'u' khác 'ú' và 'ầ'; không tự loại dấu tiếng Việt.
        var cQuery = from s in mangChuoi where s.Contains('u') select s;
        var cMethod = mangChuoi.Where(s => s.Contains('u'));
        InDay("2.2c - Query: chứa ký tự u", cQuery);
        InDay("2.2c - Method", cMethod);

        // Kiểm tra chuỗi rỗng trước khi truy cập ký tự ở vị trí 0.
        var dQuery = from s in mangChuoi
                     where !string.IsNullOrEmpty(s) && char.IsUpper(s[0])
                     select s;
        var dMethod = mangChuoi.Where(s => !string.IsNullOrEmpty(s) && char.IsUpper(s[0]));
        TieuDe("2.2d - Query: các từ bắt đầu bằng chữ in hoa");
        Console.WriteLine(string.Join(" ", dQuery));
        TieuDe("2.2d - Method");
        Console.WriteLine(string.Join(" ", dMethod));
    }

    static void Bai31()
    {
        int[] mangSo = { 50, 42, 12, 3, 9, 8, 1, 50, 3, 42, 85 };
        TieuDe("3.1a - Đếm phần tử");
        Console.WriteLine($"Tổng: {mangSo.Count()}; chẵn: {mangSo.Count(x => x % 2 == 0)}; lẻ: {mangSo.Count(x => x % 2 != 0)}");

        TieuDe("3.1b - Tổng, lớn nhất, nhỏ nhất");
        Console.WriteLine($"Tổng: {mangSo.Sum()}; max: {mangSo.Max()}; min: {mangSo.Min()}");

        // Distinct bỏ các giá trị lặp; Count đếm dãy sau khi bỏ lặp.
        TieuDe("3.1c - Số giá trị khác nhau");
        Console.WriteLine(mangSo.Distinct().Count());

        // Key là số dư. Các phần tử trong nhóm vẫn giữ cả giá trị trùng.
        TieuDe("3.1d - Nhóm theo số dư khi chia 5");
        var nhomDu = mangSo.GroupBy(x => x % 5).OrderBy(g => g.Key);
        foreach (var g in nhomDu)
            Console.WriteLine($"Dư {g.Key}: {string.Join(", ", g)}");
    }

    static string TuDau(string s) => s.Split(' ', StringSplitOptions.RemoveEmptyEntries).First();

    static void Bai32()
    {
        string[] monAn = { "Bún bò Huế", "Hủ tiếu heo", "Bánh canh", "Bánh mì",
            "Nước Cà phê", "Mì quảng", "Cơm tấm", "Nước Chanh dây", "Mì xào",
            "Bún riêu", "Bánh cuốn", "Mì gói", "Bún chả", "Hủ tiếu Nam vang" };

        // Tìm độ dài trước, sau đó lấy TẤT CẢ món cùng độ dài cực trị.
        int nganNhat = monAn.Min(s => s.Length);
        int daiNhat = monAn.Max(s => s.Length);
        InDay($"3.2a - Ngắn nhất ({nganNhat} ký tự)", monAn.Where(s => s.Length == nganNhat));
        InDay($"3.2a - Dài nhất ({daiNhat} ký tự)", monAn.Where(s => s.Length == daiNhat));

        TieuDe("3.2b - Nhóm theo từ đầu tiên");
        var cacNhom = monAn.GroupBy(s => TuDau(s));
        foreach (var g in cacNhom)
            Console.WriteLine($"{g.Key}: {string.Join("; ", g)}");

        TieuDe("3.2c - Số món có từ đầu là Bánh");
        Console.WriteLine(monAn.Count(s => TuDau(s) == "Bánh"));
    }

    static void Bai41()
    {
        InMon("4.1 - Dữ liệu List<MonHoc>", DuLieu.DS_Mon());
    }

    static void Bai51()
    {
        var ds = DuLieu.DS_Mon();
        var aQuery = from m in ds where m.TenMon.StartsWith("Lập trình") select m.TenMon;
        var aMethod = ds.Where(m => m.TenMon.StartsWith("Lập trình")).Select(m => m.TenMon);
        InDong("5.1a - Query: tên môn bắt đầu bằng Lập trình", aQuery);
        InDong("5.1a - Method", aMethod);

        // ThenBy là tiêu chí phụ khi bằng số tiết. Không thay bằng OrderBy lần hai.
        var bQuery = from m in ds where m.He == "CD" orderby m.SoTiet descending, m.MaMon select m;
        var bMethod = ds.Where(m => m.He == "CD").OrderByDescending(m => m.SoTiet).ThenBy(m => m.MaMon);
        InMon("5.1b - Query: hệ CD, tiết giảm, mã tăng", bQuery);
        InMon("5.1b - Method", bMethod);

        // So sánh không phân biệt hoa/thường để 'web' khớp với 'Web'.
        // Chỉ chiếu ra hai thuộc tính mà đề yêu cầu.
        var cQuery = from m in ds
                     where m.TenMon.Contains("web", StringComparison.OrdinalIgnoreCase)
                     select new { m.TenMon, m.He };
        var cMethod = ds.Where(m => m.TenMon.Contains("web", StringComparison.OrdinalIgnoreCase))
                        .Select(m => new { m.TenMon, m.He });
        InDong("5.1c - Query: tên chứa web", cQuery);
        InDong("5.1c - Method", cMethod);

        var dQuery = from m in ds where m.He == "KTV" orderby m.MaMon select m;
        var dMethod = ds.Where(m => m.He == "KTV").OrderBy(m => m.MaMon);
        InMon("5.1d - Query: hệ KTV, mã tăng", dQuery);
        InMon("5.1d - Method", dMethod);
    }

    static void Bai52()
    {
        var ds = DuLieu.DS_Mon();
        TieuDe("5.2a - Tổng số môn");
        Console.WriteLine(ds.Count());

        TieuDe("5.2b - Đếm môn bắt đầu bằng Lập trình");
        Console.WriteLine(ds.Count(m => m.TenMon.StartsWith("Lập trình")));

        TieuDe("5.2c - Tổng số tiết hệ KTV");
        Console.WriteLine(ds.Where(m => m.He == "KTV").Sum(m => (int)m.SoTiet));

        var d = ds.GroupBy(m => m.He)
                  .Select(g => new { He = NhanHe(g.Key), TongSoMon = g.Count() });
        InDong("5.2d - Tổng số môn theo hệ", d);

        var e = ds.GroupBy(m => m.SoTiet).OrderByDescending(g => g.Key)
                  .Select(g => new { SoTiet = g.Key, TongSoMon = g.Count() });
        InDong("5.2e - Thống kê theo số tiết giảm dần", e);

        // Không lấy tùy tiện một môn: nếu nhiều môn đồng hạng thì in đủ.
        int maxTiet = ds.Max(m => (int)m.SoTiet);
        InMon("5.2f - Môn có số tiết cao nhất", ds.Where(m => m.SoTiet == maxTiet));

        var thongKeHe = ds.GroupBy(m => m.He).Select(nhom => new
        {
            He = NhanHe(nhom.Key),
            TongSoMon = nhom.Count(),
            TongSoTiet = nhom.Sum(m => (int)m.SoTiet),
            CaoNhat = nhom.Max(m => (int)m.SoTiet),
            ThapNhat = nhom.Min(m => (int)m.SoTiet)
        });
        InDong("5.2g - Thống kê tổng hợp theo hệ", thongKeHe);

        InNhom("5.2h - Các môn theo từng hệ", ds.GroupBy(m => m.He));
        InNhom("5.2i - Các môn theo số tiết tăng dần", ds.GroupBy(m => m.SoTiet).OrderBy(nhom => nhom.Key));

        // HP2_1.Split('_')[0] cho kết quả HP2; chỉ áp dụng cho môn hệ KTV.
        var j = ds.Where(m => m.He == "KTV").OrderBy(m => m.MaMon)
                  .GroupBy(m => m.MaMon.Split('_')[0]).OrderBy(nhom => nhom.Key);
        InNhom("5.2j - Hệ KTV nhóm theo học phần, mã tăng", j);

        // LINQ to Objects giữ thứ tự nguồn trong mỗi nhóm, nên sắp mã trước GroupBy.
        var k = ds.Where(m => m.SoTiet > 40).OrderBy(m => m.MaMon).GroupBy(m => m.He);
        InNhom("5.2k - Nhóm theo hệ, tiết > 40, mã tăng", k);
    }

    static void Bai61()
    {
        InDong("6.1 - Dữ liệu hệ", DuLieu.DS_He().Select(h => new { h.MaHe, h.TenHe }));
    }

    static void Bai62()
    {
        var dsMon = DuLieu.DS_Mon();
        var dsHe = DuLieu.DS_He();

        // a: inner join chỉ giữ các cặp có khóa khớp (MaHe == He).
        var aQuery = from h in dsHe
                     join m in dsMon on h.MaHe equals m.He
                     select new { h.TenHe, m.MaMon, m.TenMon };
        var aMethod = dsHe.Join(dsMon, h => h.MaHe, m => m.He,
            (h, m) => new { h.TenHe, m.MaMon, m.TenMon });
        InDong("6.2a - Query: inner join", aQuery);
        InDong("6.2a - Method: inner join", aMethod);

        // b: GroupJoin tạo nhóm môn cho MỖI hệ, kể cả nhóm rỗng.
        // DefaultIfEmpty tạo một phần tử null cho nhóm rỗng.
        // SelectMany trải các nhóm thành từng dòng; ?. và ?? xử lý môn null.
        var left = dsHe.GroupJoin(dsMon, h => h.MaHe, m => m.He,
                (h, nhomMon) => new { He = h, Mon = nhomMon })
            .SelectMany(x => x.Mon.DefaultIfEmpty(), (x, m) => new
            {
                MaHe = x.He.MaHe,
                TenHe = x.He.TenHe,
                MaMon = m?.MaMon ?? "(không có môn)",
                TenMon = m?.TenMon ?? "(không có môn)"
            });
        InDong("6.2b - Left join: giữ cả hệ chưa có môn", left);

        // c: full outer join = left join + những môn KHÔNG khớp bất kỳ hệ nào.
        // Hai nguồn Concat ở đây không chồng lên nhau, nên không lặp cặp khớp.
        var monKhongCoHe = dsMon.Where(m => !dsHe.Any(h => h.MaHe == m.He))
            .Select(m => new
            {
                MaHe = NhanHe(m.He),
                TenHe = "(chưa khai báo hệ)",
                MaMon = m.MaMon,
                TenMon = m.TenMon
            });
        InDong("6.2c - Đủ hệ và môn, kể cả hai phía không khớp", left.Concat(monKhongCoHe));

        // d: chỉ giữ hai phần không khớp, không lấy 17 môn có hệ hợp lệ.
        var heKhongCoMon = dsHe.Where(h => !dsMon.Any(m => m.He == h.MaHe))
            .Select(h => new
            {
                MaHe = h.MaHe,
                TenHe = h.TenHe,
                MaMon = "(không có môn)",
                TenMon = "(không có môn)"
            });
        InDong("6.2d - Chỉ hệ chưa có môn và môn chưa có hệ", heKhongCoMon.Concat(monKhongCoHe));

        // e: sắp xếp rồi mới Take(5). Khi bằng số tiết, giữ thứ tự dữ liệu gốc.
        // Bắt đầu từ môn để không loại môn thiếu hệ trước khi lấy top 5.
        var top5 = dsMon.OrderByDescending(m => m.SoTiet).Take(5);
        var e = from m in top5
                join h in dsHe on m.He equals h.MaHe into nhomHe
                from h in nhomHe.DefaultIfEmpty()
                select new { TenHe = h?.TenHe ?? "(chưa khai báo hệ)", m.MaMon, m.TenMon, m.SoTiet };
        InDong("6.2e - Năm môn có số tiết giảm dần", e);

        // f: đếm nhóm gốc của GroupJoin; hệ QT có đúng 0 môn.
        // Không đếm phần tử null do DefaultIfEmpty tạo ra.
        var f = dsHe.GroupJoin(dsMon, h => h.MaHe, m => m.He,
            (h, nhomMon) => new { h.MaHe, h.TenHe, TongSoMon = nhomMon.Count() });
        InDong("6.2f - Tổng số môn của từng hệ đã khai báo", f);

        TieuDe("6.2g - Số loại số tiết khác nhau");
        Console.WriteLine(dsMon.Select(m => m.SoTiet).Distinct().Count());

        // h: đầu tiên theo thứ tự dữ liệu nguồn, không tự sắp xếp trước.
        var dauTien = dsMon.FirstOrDefault(m => m.TenMon.StartsWith("Lập trình"));
        TieuDe("6.2h - Môn đầu tiên có tên bắt đầu bằng Lập trình");
        Console.WriteLine(dauTien == null ? "Không tìm thấy" :
            $"{dauTien.MaMon} | {dauTien.TenMon} | {dauTien.He} | {dauTien.SoTiet} tiết");

        // i: chỉ số i của Select bắt đầu từ 0 trong TỪNG nhóm => STT = i + 1.
        // Xuất cả hệ rỗng QT và nhóm môn chưa có hệ để thấy đủ dữ liệu.
        TieuDe("6.2i - Danh sách môn theo hệ, đánh STT trong mỗi nhóm");
        var cacHe = dsHe.GroupJoin(dsMon, h => h.MaHe, m => m.He,
            (h, nhomMon) => new { h.MaHe, h.TenHe, Mon = nhomMon.ToList() });
        foreach (var he in cacHe)
        {
            Console.WriteLine($"{he.MaHe} - {he.TenHe} ({he.Mon.Count} môn)");
            if (he.Mon.Count == 0)
                Console.WriteLine("  (chưa có môn học)");
            var danhSo = he.Mon.Select((m, i) => new { STT = i + 1, Mon = m });
            foreach (var dong in danhSo)
                Console.WriteLine($"  {dong.STT}. {dong.Mon.MaMon} | {dong.Mon.TenMon} | {dong.Mon.SoTiet} tiết");
        }
        var monLe = dsMon.Where(m => !dsHe.Any(h => h.MaHe == m.He)).ToList();
        if (monLe.Any())
        {
            Console.WriteLine("Môn chưa khai báo hệ");
            var danhSo = monLe.Select((m, i) => new { STT = i + 1, Mon = m });
            foreach (var dong in danhSo)
                Console.WriteLine($"  {dong.STT}. {dong.Mon.MaMon} | {dong.Mon.TenMon} | {dong.Mon.SoTiet} tiết");
        }
    }
}
