using System.Text;

namespace MinhHoaCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding =
                Encoding.UTF8;

            Console.OutputEncoding =
                Encoding.UTF8;

            while (true)
            {
                Console.Clear();

                Console.WriteLine(
                    "===================================="
                );

                Console.WriteLine(
                    "     MINH HỌA COLLECTION TRONG C#"
                );

                Console.WriteLine(
                    "===================================="
                );

                Console.WriteLine(
                    "1. Dictionary - Đếm từ trong chuỗi"
                );

                Console.WriteLine(
                    "2. SortedList"
                );

                Console.WriteLine(
                    "3. SortedSet<Person> + IComparer"
                );

                Console.WriteLine(
                    "0. Thoát"
                );

                Console.WriteLine(
                    "===================================="
                );

                Console.Write("Chọn: ");

                string choice =
                    Console.ReadLine() ?? "";

                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        DemTuTrongChuoi.Run();
                        break;

                    case "2":
                        MinhHoaSortedList.Run();
                        break;

                    case "3":
                        SortedPerson.Run();
                        break;

                    case "0":
                        Console.WriteLine(
                            "Kết thúc chương trình."
                        );
                        return;

                    default:
                        Console.WriteLine(
                            "Lựa chọn không hợp lệ."
                        );
                        break;
                }

                Console.WriteLine(
                    "\nNhấn Enter để quay lại menu..."
                );

                Console.ReadLine();
            }
        }
    }
}