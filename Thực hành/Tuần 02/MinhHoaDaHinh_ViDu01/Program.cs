using System;
using System.Text;

namespace MinhHoaDaHinh_ViDu01
{
    // Lớp cha
    class A
    {
        // Phương thức của lớp A
        public void Method1()
        {
            Console.WriteLine("A - Method1");
        }

        // Phương thức bình thường, KHÔNG virtual
        public void Process()
        {
            Console.WriteLine("A - Process");
        }

        // Cho phép lớp con override
        public virtual void Name()
        {
            Console.WriteLine("Tôi là A!");
        }
    }

    // B kế thừa A
    class B : A
    {
        // B có thêm chức năng riêng
        public void Method2()
        {
            Console.WriteLine("B - Method2");
        }

        // Process trùng tên với A.Process
        // nhưng không phải override vì A.Process không virtual
        public new void Process()
        {
            Console.WriteLine("B - Process");
        }

        // Ghi đè phương thức Name của A
        public override void Name()
        {
            Console.WriteLine("Tôi là B!");
        }
    }

    // C kế thừa B
    class C : B
    {
        public override void Name()
        {
            Console.WriteLine("Tôi là C!");
        }
    }

    // D kế thừa C
    class D : C
    {
        // Cố ý tạo một virtual method mới,
        // che phương thức Name kế thừa từ C
        public new virtual void Name()
        {
            Console.WriteLine("Tôi là D!");
        }
    }

    // E kế thừa D
    class E : D
    {
        // Override Name mới của D
        public override void Name()
        {
            Console.WriteLine("Tôi là E!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("===== 1. TÁI SỬ DỤNG =====");

            B b = new B();

            // B kế thừa Method1 của A
            b.Method1();


            Console.WriteLine("\n===== 2. MỞ RỘNG =====");

            // Method2 chỉ có trong B
            b.Method2();


            Console.WriteLine("\n===== 3. KHÔNG ĐA HÌNH =====");

            // Biến kiểu A nhưng đối tượng thật là B
            A a = new B();

            // Process không virtual
            // nên xét theo kiểu của biến tham chiếu A
            a.Process();


            Console.WriteLine("\n===== 4. ĐA HÌNH =====");

            A a1 = new B();

            // Name là virtual/override
            // nên xét theo đối tượng thật là B
            a1.Name();


            Console.WriteLine("\n===== 5. ĐA HÌNH QUA NHIỀU LỚP =====");

            A aa = new A();
            A bb = new B();
            A cc = new C();
            A dd = new D();
            A ee = new E();

            Console.Write("aa.Name(): ");
            aa.Name();

            Console.Write("bb.Name(): ");
            bb.Name();

            Console.Write("cc.Name(): ");
            cc.Name();

            Console.Write("dd.Name(): ");
            dd.Name();

            Console.Write("ee.Name(): ");
            ee.Name();


            Console.WriteLine("\n===== 6. GỌI TRỰC TIẾP D VÀ E =====");

            D d = new D();
            E e = new E();

            Console.Write("d.Name(): ");
            d.Name();

            Console.Write("e.Name(): ");
            e.Name();
        }
    }
}