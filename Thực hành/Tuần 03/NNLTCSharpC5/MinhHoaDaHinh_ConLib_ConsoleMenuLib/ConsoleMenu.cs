namespace NNLTCS.UTILS.ConsoleLib
{
    // Delegate dùng cho Event MenuChoose
    public delegate void MenuChooseEventHandler(
        object sender,
        MenuChooseEventArgs args
    );

    public class ConsoleMenu
    {
        private string title;
        private string footer;

        private List<string> dsItems;

        // Cách xử lý menu bằng Interface
        private IMenuChooseListener? menuChooseListener = null;

        // Cách xử lý menu bằng Event
        public event MenuChooseEventHandler? MenuChoose;

        public ConsoleMenu(
            string title,
            string footer,
            params string[] items
        )
        {
            this.title = title;
            this.footer = footer;

            dsItems = new List<string>();

            for (int i = 0; i < items.Length; i++)
            {
                dsItems.Add(items[i]);
            }
        }

        // Hiển thị menu và lấy lựa chọn
        public int ShowMenu()
        {
            Console.Clear();

            Console.WriteLine(title);

            Console.WriteLine(
                "========================================"
            );

            for (int i = 0; i < dsItems.Count; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {dsItems[i]}"
                );
            }

            Console.WriteLine(
                "========================================"
            );

            Console.Write(footer);

            int chon = int.Parse(
                Console.ReadLine()!
            );

            Console.WriteLine(
                "========================================"
            );

            return chon;
        }

        // Đăng ký listener xử lý menu bằng Interface
        public void AddMenuChooseListener(
            IMenuChooseListener listener
        )
        {
            menuChooseListener = listener;
        }

        // Phương thức virtual
        // Lớp con có thể override
        public virtual void OnMenuChoose(
            MenuChooseEventArgs args
        )
        {
            Console.WriteLine(
                $"Bạn đang chọn chức năng {args.Item}: {args.ItemText}"
            );
        }

        public void Process()
        {
            int chon;

            do
            {
                chon = ShowMenu();

                MenuChooseEventArgs args;

                if (chon < 1 || chon > dsItems.Count)
                {
                    args = new MenuChooseEventArgs(
                        chon,
                        string.Empty
                    );
                }
                else
                {
                    args = new MenuChooseEventArgs(
                        chon,
                        dsItems[chon - 1]
                    );
                }

                // Ưu tiên xử lý bằng Event
                if (MenuChoose != null)
                {
                    MenuChoose(this, args);
                }

                // Nếu không có Event thì dùng Interface
                else if (menuChooseListener != null)
                {
                    menuChooseListener.MenuChoose(
                        this,
                        args
                    );
                }

                // Cuối cùng dùng virtual method
                else
                {
                    OnMenuChoose(args);
                }

                if (args.IsExit)
                {
                    break;
                }

                Console.WriteLine();

                Console.WriteLine(
                    "Nhấn Enter để tiếp tục..."
                );

                Console.ReadLine();

            } while (true);
        }
    }
}