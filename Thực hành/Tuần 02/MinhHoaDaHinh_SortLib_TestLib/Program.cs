using System.Collections;
using System.Text;
using MinhHoaDaHinh_SortLib;

namespace MinhHoaDaHinh_SortLib_TestLib
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Test1();
            Test2();
            Test3();
            Test4();
            Test5();
        }

        static void XuatMang(IList ds)
        {
            Console.Write("Dãy số: ");

            for (int i = 0; i < ds.Count; i++)
            {
                Console.Write($"{ds[i]} ");
            }

            Console.WriteLine();
        }

        // Test 1: sắp tăng hard-code
        static void Test1()
        {
            Console.WriteLine(
                "\n===== TEST 1: SẮP TĂNG ====="
            );

            int[] ds =
            {
                1, 5, 2, 1, 3, 4, 2, 3
            };

            Console.WriteLine("Trước:");
            XuatMang(ds);

            SortLib.SapTang(ds);

            Console.WriteLine("Sau:");
            XuatMang(ds);
        }

        // Test 2: adapter mặc định - tăng dần
        static void Test2()
        {
            Console.WriteLine(
                "\n===== TEST 2: ADAPTER TĂNG DẦN ====="
            );

            int[] ds =
            {
                1, 5, 2, 1, 3, 4, 2, 3
            };

            Console.WriteLine("Trước:");
            XuatMang(ds);

            SortLib.SapXep(
                ds,
                new SapXepAdapter()
            );

            Console.WriteLine("Sau:");
            XuatMang(ds);
        }

        // Test 3: đa hình - sắp giảm
        static void Test3()
        {
            Console.WriteLine(
                "\n===== TEST 3: ADAPTER GIẢM DẦN ====="
            );

            int[] ds =
            {
                1, 5, 2, 1, 3, 4, 2, 3
            };

            Console.WriteLine("Trước:");
            XuatMang(ds);

            SortLib.SapXep(
                ds,
                new SapGiamAdapter()
            );

            Console.WriteLine("Sau:");
            XuatMang(ds);
        }

        // Test 4: đa hình - chẵn trước, lẻ sau
        static void Test4()
        {
            Console.WriteLine(
                "\n===== TEST 4: CHẴN TRƯỚC - LẺ SAU ====="
            );

            int[] ds =
            {
                1, 5, 2, 1, 3, 4, 2, 3
            };

            Console.WriteLine("Trước:");
            XuatMang(ds);

            SortLib.SapXep(
                ds,
                new ChanLeAdapter()
            );

            Console.WriteLine("Sau:");
            XuatMang(ds);
        }

        // Test 5: sắp xếp mảng double thông qua IList
        static void Test5()
        {
            Console.WriteLine(
                "\n===== TEST 5: SẮP XẾP MẢNG DOUBLE ====="
            );

            double[] ds =
            {
                1.1,
                5.1,
                2.2,
                1.3,
                3.2,
                4.5,
                2.3,
                3.3
            };

            Console.WriteLine("Trước:");
            XuatMang(ds);

            SortLib.SapXep(
                ds,
                new SapTangDoubleAdapter()
            );

            Console.WriteLine("Sau:");
            XuatMang(ds);
        }
    }
}