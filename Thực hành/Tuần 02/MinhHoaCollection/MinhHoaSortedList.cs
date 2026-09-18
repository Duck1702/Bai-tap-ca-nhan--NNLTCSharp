using System.Collections;

namespace MinhHoaCollection
{
    public static class MinhHoaSortedList
    {
        public static void Run()
        {
            Console.WriteLine(
                "===== DEMO SORTEDLIST ====="
            );

            SortedList danhSach = new SortedList();

            danhSach.Add("003", "Nguyễn Văn C");
            danhSach.Add("001", "Nguyễn Văn A");
            danhSach.Add("005", "Nguyễn Văn E");
            danhSach.Add("002", "Nguyễn Văn B");
            danhSach.Add("004", "Nguyễn Văn D");

            Console.WriteLine(
                "\nDanh sách sau khi thêm:"
            );

            ICollection keys = danhSach.Keys;

            foreach (string key in keys)
            {
                Console.WriteLine(
                    $"{key}: {danhSach[key]}"
                );
            }

            Console.WriteLine();

            if (danhSach.ContainsKey("003"))
            {
                Console.WriteLine(
                    "Tìm thấy sinh viên mã 003."
                );
            }

            if (!danhSach.ContainsKey("006"))
            {
                danhSach.Add(
                    "006",
                    "Nguyễn Văn F"
                );

                Console.WriteLine(
                    "Đã thêm sinh viên mã 006."
                );
            }

            Console.WriteLine(
                "\nDanh sách cuối cùng:"
            );

            foreach (string key in danhSach.Keys)
            {
                Console.WriteLine(
                    $"{key}: {danhSach[key]}"
                );
            }
        }
    }
}