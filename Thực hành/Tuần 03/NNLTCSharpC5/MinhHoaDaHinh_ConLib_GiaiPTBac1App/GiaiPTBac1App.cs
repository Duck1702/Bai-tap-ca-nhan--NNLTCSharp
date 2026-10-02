using NNLTCS.UTILS.ConsoleLib;

namespace NNLTCS.BaiTap.GiaiPTBac1App
{
    class GiaiPTBac1App : ConsoleMenu
    {
        private double a;
        private double b;

        public GiaiPTBac1App()
            : base(
                "GIẢI PHƯƠNG TRÌNH BẬC 1",
                "Mời bạn chọn: ",
                "Nhập tham số a, b",
                "Xuất tham số a, b",
                "Giải phương trình",
                "Thoát"
            )
        {
        }

        public override void OnMenuChoose(
            MenuChooseEventArgs args
        )
        {
            switch (args.Item)
            {
                case 1:
                    Console.Write("Nhập a: ");
                    a = double.Parse(
                        Console.ReadLine()!
                    );

                    Console.Write("Nhập b: ");
                    b = double.Parse(
                        Console.ReadLine()!
                    );
                    break;

                case 2:
                    Console.WriteLine(
                        $"{a}x + {b} = 0"
                    );
                    break;

                case 3:
                    if (a == 0)
                    {
                        if (b == 0)
                        {
                            Console.WriteLine(
                                "Phương trình vô số nghiệm."
                            );
                        }
                        else
                        {
                            Console.WriteLine(
                                "Phương trình vô nghiệm."
                            );
                        }
                    }
                    else
                    {
                        Console.WriteLine(
                            $"Phương trình có nghiệm x = {-b / a}"
                        );
                    }
                    break;

                case 4:
                    Console.WriteLine(
                        "Hẹn gặp lại!"
                    );

                    args.IsExit = true;
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