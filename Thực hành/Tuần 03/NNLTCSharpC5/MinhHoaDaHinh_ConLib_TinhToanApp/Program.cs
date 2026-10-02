using NNLTCS.UTILS.ConsoleLib;

namespace NNLTCS.BaiTap.TinhToanApp
{
    class XuLyMenuChoose : IMenuChooseListener
    {
        public void MenuChoose(
            object sender,
            MenuChooseEventArgs args
        )
        {
            switch (args.Item)
            {
                case 1:
                    Console.WriteLine("Thoát");
                    args.IsExit = true;
                    break;

                case 2:
                    Console.WriteLine("Nhập");
                    break;

                case 3:
                    Console.WriteLine("Tính");
                    break;

                default:
                    break;
            }
        }
    }

    public class TinhToanApp
    {
        public static double a;
        public static double b;

        static void Main(string[] args)
        {
            ConsoleMenu app = new ConsoleMenu(
                "TÍNH TỔNG HAI SỐ",
                "Mời bạn chọn: ",
                "Thoát",
                "Nhập số a, b",
                "Xuất a, b",
                "Tính tổng"
            );

            // Cách 1: xử lý bằng Interface
            // app.AddMenuChooseListener(
            //     new XuLyMenuChoose()
            // );

            // Cách 2: xử lý bằng Event
            app.MenuChoose += App_MenuChoose;

            app.Process();
        }

        static void App_MenuChoose(
            object sender,
            MenuChooseEventArgs args
        )
        {
            Console.WriteLine("Event");

            switch (args.Item)
            {
                case 1:
                    Console.WriteLine("Thoát");
                    args.IsExit = true;
                    break;

                case 2:
                    Console.Write("Nhập a: ");
                    a = double.Parse(
                        Console.ReadLine()!
                    );

                    Console.Write("Nhập b: ");
                    b = double.Parse(
                        Console.ReadLine()!
                    );
                    break;

                case 3:
                    Console.WriteLine(
                        $"a = {a}, b = {b}"
                    );
                    break;

                case 4:
                    Console.WriteLine(
                        $"{a} + {b} = {a + b}"
                    );
                    break;

                default:
                    Console.WriteLine(
                        "Lựa chọn không hợp lệ."
                    );
                    break;
            }
        }
    }
}