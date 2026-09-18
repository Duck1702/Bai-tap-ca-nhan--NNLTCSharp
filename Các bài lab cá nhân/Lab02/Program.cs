using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NNLTCS_ThucHanh02
{
    // =========================================================
    // PHẦN 1 - THIẾT KẾ LỚP CƠ BẢN
    // =========================================================

    // ---------------------------------------------------------
    // BÀI 1.1 - TÍNH TUỔI SINH VIÊN
    // ---------------------------------------------------------
    public class StudentAge
    {
        private string fullName;
        private int birthYear;

        public string FullName
        {
            get => fullName;
            set => fullName = value ?? "";
        }

        public int BirthYear
        {
            get => birthYear;
            set => birthYear = value;
        }

        public StudentAge()
        {
            fullName = "";
            birthYear = DateTime.Now.Year;
        }

        public StudentAge(string fullName, int birthYear)
        {
            this.fullName = fullName;
            this.birthYear = birthYear;
        }

        public void Input()
        {
            Console.Write("Nhập họ tên: ");
            fullName = Console.ReadLine() ?? "";

            birthYear = InputHelper.ReadInt(
                "Nhập năm sinh: ",
                min: 1,
                max: DateTime.Now.Year
            );
        }

        public int GetAge()
        {
            // Đề chỉ cho năm sinh nên tuổi được tính theo năm.
            return DateTime.Now.Year - birthYear;
        }

        public void Output()
        {
            Console.WriteLine($"Họ tên: {fullName}");
            Console.WriteLine($"Năm sinh: {birthYear}");
            Console.WriteLine($"Tuổi: {GetAge()}");
        }
    }

    // ---------------------------------------------------------
    // BÀI 1.2 - LỚP POINT
    // ---------------------------------------------------------
    public class Point
    {
        private double x;
        private double y;

        public double X
        {
            get => x;
            set => x = value;
        }

        public double Y
        {
            get => y;
            set => y = value;
        }

        public Point()
        {
            x = 0;
            y = 0;
        }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(Point other)
        {
            x = other.x;
            y = other.y;
        }

        public void Input()
        {
            x = InputHelper.ReadDouble("Nhập x = ");
            y = InputHelper.ReadDouble("Nhập y = ");
        }

        public void Output()
        {
            Console.WriteLine(ToString());
        }

        public override string ToString()
        {
            return $"({x:0.##}, {y:0.##})";
        }

        // Cộng theo từng tọa độ.
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.x + b.x, a.y + b.y);
        }

        // Trừ theo từng tọa độ.
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.x - b.x, a.y - b.y);
        }

        // Lấy điểm đối.
        public static Point operator -(Point a)
        {
            return new Point(-a.x, -a.y);
        }

        // Cách 1: phương thức thành viên.
        public double DistanceTo(Point other)
        {
            double dx = x - other.x;
            double dy = y - other.y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // Cách 2: phương thức tĩnh.
        public static double Distance(Point a, Point b)
        {
            return a.DistanceTo(b);
        }

        // Cách 1: phương thức thành viên.
        public Point MidPoint(Point other)
        {
            return new Point(
                (x + other.x) / 2.0,
                (y + other.y) / 2.0
            );
        }

        // Cách 2: phương thức tĩnh.
        public static Point MidPoint(Point a, Point b)
        {
            return a.MidPoint(b);
        }
    }

    // ---------------------------------------------------------
    // BÀI 1.3 - LỚP PERSON
    // ---------------------------------------------------------
    public class Person
    {
        private string id;
        private string name;
        private int yob; // year of birth
        private int yod; // year of death, 0 = còn sống

        public string Id
        {
            get => id;
            set => id = value ?? "";
        }

        public string Name
        {
            get => name;
            set => name = value ?? "";
        }

        public int Yob
        {
            get => yob;
            set => yob = value;
        }

        public int Yod
        {
            get => yod;
            set => yod = value;
        }

        public Person()
        {
            id = "";
            name = "";
            yob = 0;
            yod = 0;
        }

        public Person(string id, string name, int yob, int yod = 0)
        {
            this.id = id;
            this.name = name;
            this.yob = yob;
            this.yod = yod;
        }

        // Copy constructor.
        public Person(Person other)
        {
            id = other.id;
            name = other.name;
            yob = other.yob;
            yod = other.yod;
        }

        public void Input()
        {
            Console.Write("ID: ");
            id = Console.ReadLine() ?? "";

            Console.Write("Họ tên: ");
            name = Console.ReadLine() ?? "";

            yob = InputHelper.ReadInt("Năm sinh: ", min: 1);

            while (true)
            {
                yod = InputHelper.ReadInt("Năm mất (0 nếu còn sống): ", min: 0);

                if (yod == 0 || yod >= yob)
                    break;

                Console.WriteLine("Năm mất phải bằng 0 hoặc >= năm sinh.");
            }
        }

        public bool IsLiving()
        {
            return yod == 0;
        }

        public void Output()
        {
            Console.WriteLine($"ID: {id}");
            Console.WriteLine($"Họ tên: {name}");
            Console.WriteLine($"Năm sinh: {yob}");

            if (IsLiving())
                Console.WriteLine("Trạng thái: Còn sống");
            else
                Console.WriteLine($"Năm mất: {yod}");
        }

        public override string ToString()
        {
            return $"{id} - {name} - {yob} - {(IsLiving() ? "Còn sống" : $"Mất năm {yod}")}";
        }
    }

    // ---------------------------------------------------------
    // BÀI 1.4 - LỚP PHÂN SỐ
    // ---------------------------------------------------------
    public class Fraction : IEquatable<Fraction>
    {
        private int numerator;
        private int denominator;

        public int Numerator => numerator;
        public int Denominator => denominator;

        // 0/1
        public Fraction()
        {
            numerator = 0;
            denominator = 1;
        }

        // n/1
        public Fraction(int n)
        {
            numerator = n;
            denominator = 1;
        }

        // n/d
        public Fraction(int n, int d)
        {
            if (d == 0)
                throw new ArgumentException("Mẫu số không được bằng 0.");

            numerator = n;
            denominator = d;
            Normalize();
        }

        // Copy constructor.
        public Fraction(Fraction other)
        {
            numerator = other.numerator;
            denominator = other.denominator;
        }

        private static int Gcd(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (b != 0)
            {
                int r = a % b;
                a = b;
                b = r;
            }

            return a == 0 ? 1 : a;
        }

        private void Normalize()
        {
            // Đưa dấu âm lên tử.
            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

            int gcd = Gcd(numerator, denominator);
            numerator /= gcd;
            denominator /= gcd;
        }

        public override string ToString()
        {
            return denominator == 1
                ? numerator.ToString()
                : $"{numerator}/{denominator}";
        }

        // Toán tử một ngôi.
        public static Fraction operator +(Fraction a)
        {
            return new Fraction(a);
        }

        public static Fraction operator -(Fraction a)
        {
            return new Fraction(-a.numerator, a.denominator);
        }

        // Toán tử hai ngôi.
        public static Fraction operator +(Fraction a, Fraction b)
        {
            return new Fraction(
                a.numerator * b.denominator + b.numerator * a.denominator,
                a.denominator * b.denominator
            );
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return new Fraction(
                a.numerator * b.denominator - b.numerator * a.denominator,
                a.denominator * b.denominator
            );
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(
                a.numerator * b.numerator,
                a.denominator * b.denominator
            );
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (b.numerator == 0)
                throw new DivideByZeroException("Không thể chia cho phân số 0.");

            return new Fraction(
                a.numerator * b.denominator,
                a.denominator * b.numerator
            );
        }

        private static long LeftCross(Fraction a, Fraction b)
        {
            return (long)a.numerator * b.denominator;
        }

        private static long RightCross(Fraction a, Fraction b)
        {
            return (long)b.numerator * a.denominator;
        }

        public static bool operator >(Fraction a, Fraction b)
            => LeftCross(a, b) > RightCross(a, b);

        public static bool operator <(Fraction a, Fraction b)
            => LeftCross(a, b) < RightCross(a, b);

        public static bool operator >=(Fraction a, Fraction b)
            => LeftCross(a, b) >= RightCross(a, b);

        public static bool operator <=(Fraction a, Fraction b)
            => LeftCross(a, b) <= RightCross(a, b);

        public static bool operator ==(Fraction? a, Fraction? b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null || b is null)
                return false;

            return a.numerator == b.numerator
                && a.denominator == b.denominator;
        }

        public static bool operator !=(Fraction? a, Fraction? b)
        {
            return !(a == b);
        }

        public bool Equals(Fraction? other)
        {
            return other is not null && this == other;
        }

        public override bool Equals(object? obj)
        {
            return obj is Fraction other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(numerator, denominator);
        }
    }

    // ---------------------------------------------------------
    // BÀI 1.5 - LỚP ĐƠN THỨC
    // ---------------------------------------------------------
    public class Monomial
    {
        private double coefficient; // a
        private int exponent;       // n >= 0

        public double Coefficient
        {
            get => coefficient;
            set => coefficient = value;
        }

        public int Exponent
        {
            get => exponent;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Số mũ phải >= 0.");

                exponent = value;
            }
        }

        public Monomial()
        {
            coefficient = 0;
            exponent = 0;
        }

        public Monomial(double coefficient, int exponent)
        {
            if (exponent < 0)
                throw new ArgumentException("Số mũ phải >= 0.");

            this.coefficient = coefficient;
            this.exponent = exponent;
        }

        public Monomial(Monomial other)
        {
            coefficient = other.coefficient;
            exponent = other.exponent;
        }

        public double Evaluate(double x)
        {
            return coefficient * Math.Pow(x, exponent);
        }

        public Monomial Derivative()
        {
            if (exponent == 0)
                return new Monomial(0, 0);

            return new Monomial(
                coefficient * exponent,
                exponent - 1
            );
        }

        public override string ToString()
        {
            if (Math.Abs(coefficient) < 1e-12)
                return "0";

            if (exponent == 0)
                return $"{coefficient:0.##}";

            if (exponent == 1)
                return $"{coefficient:0.##}x";

            return $"{coefficient:0.##}x^{exponent}";
        }
    }

    // =========================================================
    // PHẦN 2 - LỚP CHỨA MẢNG, LIST, INDEXER
    // =========================================================

    // ---------------------------------------------------------
    // BÀI 2.1 - ARRAYPOINT
    // Đề yêu cầu dùng ArrayList để chứa Point.
    // ---------------------------------------------------------
    public class ArrayPoint
    {
        private readonly ArrayList points;

        public ArrayPoint()
        {
            points = new ArrayList();
        }

        public ArrayPoint(ArrayPoint other)
        {
            points = new ArrayList();

            foreach (Point p in other.points)
                points.Add(new Point(p));
        }

        public int Count => points.Count;

        public Point this[int index]
        {
            get
            {
                CheckIndex(index);
                return (Point)points[index]!;
            }
            set
            {
                CheckIndex(index);
                points[index] = value;
            }
        }

        private void CheckIndex(int index)
        {
            if (index < 0 || index >= points.Count)
                throw new IndexOutOfRangeException();
        }

        public void Add(Point point)
        {
            points.Add(point);
        }

        public void Input()
        {
            int n = InputHelper.ReadInt("Nhập số điểm: ", min: 0);

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Điểm thứ {i + 1}:");
                Point p = new Point();
                p.Input();
                points.Add(p);
            }
        }

        public void Output()
        {
            for (int i = 0; i < points.Count; i++)
                Console.WriteLine($"[{i}] = {points[i]}");
        }
    }

    // ---------------------------------------------------------
    // BÀI 2.2 - PERSONLIST
    // ---------------------------------------------------------
    public class PersonList
    {
        private readonly List<Person> people;

        public PersonList()
        {
            people = new List<Person>();
        }

        public PersonList(PersonList other)
        {
            people = new List<Person>();

            foreach (Person p in other.people)
                people.Add(new Person(p));
        }

        public int Count => people.Count;

        public Person this[int index]
        {
            get => people[index];
            set => people[index] = value;
        }

        public void Add(Person x)
        {
            people.Add(x);
        }

        public void Input()
        {
            int n = InputHelper.ReadInt("Nhập số người: ", min: 0);

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"\nNgười thứ {i + 1}:");
                Person p = new Person();
                p.Input();
                people.Add(p);
            }
        }

        public void Output()
        {
            if (people.Count == 0)
            {
                Console.WriteLine("Danh sách rỗng.");
                return;
            }

            for (int i = 0; i < people.Count; i++)
            {
                Console.WriteLine($"\n--- Người {i + 1} ---");
                people[i].Output();
            }
        }

        public PersonList LivingPeople()
        {
            PersonList result = new PersonList();

            foreach (Person p in people)
            {
                if (p.IsLiving())
                    result.Add(new Person(p));
            }

            return result;
        }
    }

    // ---------------------------------------------------------
    // BÀI 2.3 (MẢNG 1 CHIỀU)
    // ---------------------------------------------------------
    public class IntArray1D
    {
        private int[] data;

        public int Length => data.Length;

        public IntArray1D()
        {
            data = Array.Empty<int>();
        }

        public IntArray1D(int n)
        {
            if (n < 0)
                throw new ArgumentException("Kích thước phải >= 0.");

            data = new int[n];
        }

        public IntArray1D(int[] source)
        {
            data = (int[])source.Clone();
        }

        public IntArray1D(IntArray1D other)
        {
            data = (int[])other.data.Clone();
        }

        public int this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }

        public void Input()
        {
            for (int i = 0; i < data.Length; i++)
                data[i] = InputHelper.ReadInt($"a[{i}] = ");
        }

        public void Output()
        {
            Console.WriteLine(string.Join(" ", data));
        }

        public List<int> FindEvenNumbers()
        {
            List<int> result = new List<int>();

            foreach (int value in data)
            {
                if (value % 2 == 0)
                    result.Add(value);
            }

            return result;
        }
    }

    // ---------------------------------------------------------
    // BÀI 2.4 (MẢNG 2 CHIỀU)
    // ---------------------------------------------------------
    public class IntMatrix2D
    {
        private int[,] data;

        public int Rows => data.GetLength(0);
        public int Columns => data.GetLength(1);

        public IntMatrix2D()
        {
            data = new int[0, 0];
        }

        public IntMatrix2D(int rows, int columns)
        {
            if (rows < 0 || columns < 0)
                throw new ArgumentException("Kích thước phải >= 0.");

            data = new int[rows, columns];
        }

        public IntMatrix2D(IntMatrix2D other)
        {
            data = (int[,])other.data.Clone();
        }

        public int this[int i, int j]
        {
            get => data[i, j];
            set => data[i, j] = value;
        }

        public void Input()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                    data[i, j] = InputHelper.ReadInt($"a[{i},{j}] = ");
            }
        }

        public void Output()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                    Console.Write($"{data[i, j],8}");

                Console.WriteLine();
            }
        }

        private static bool IsPrime(int n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            if (n % 2 == 0)
                return false;

            for (int i = 3; i <= n / i; i += 2)
            {
                if (n % i == 0)
                    return false;
            }

            return true;
        }

        public List<int> FindPrimes()
        {
            List<int> result = new List<int>();

            foreach (int value in data)
            {
                if (IsPrime(value))
                    result.Add(value);
            }

            return result;
        }
    }

    // ---------------------------------------------------------
    // BÀI 2.5 (ĐA THỨC )
    // P(x) = a0 + a1*x + ... + an*x^n
    // ---------------------------------------------------------
    public class Polynomial
    {
        private readonly Monomial[] terms;

        public int Degree => terms.Length - 1;

        public Polynomial(int degree)
        {
            if (degree < 0)
                throw new ArgumentException("Bậc đa thức phải >= 0.");

            terms = new Monomial[degree + 1];

            for (int i = 0; i < terms.Length; i++)
                terms[i] = new Monomial(0, i);
        }

        public Polynomial(Polynomial other)
        {
            terms = new Monomial[other.terms.Length];

            for (int i = 0; i < terms.Length; i++)
                terms[i] = new Monomial(other.terms[i]);
        }

        public Monomial this[int index]
        {
            get => terms[index];
            set
            {
                if (value.Exponent != index)
                {
                    // Vì phần tử thứ i biểu diễn a_i*x^i.
                    terms[index] = new Monomial(value.Coefficient, index);
                }
                else
                {
                    terms[index] = value;
                }
            }
        }

        public void Input()
        {
            for (int i = 0; i < terms.Length; i++)
            {
                double coefficient =
                    InputHelper.ReadDouble($"Nhập hệ số a{i}: ");

                terms[i] = new Monomial(coefficient, i);
            }
        }

        public double Evaluate(double x)
        {
            double sum = 0;

            foreach (Monomial term in terms)
                sum += term.Evaluate(x);

            return sum;
        }

        public void Output()
        {
            bool first = true;

            for (int i = Degree; i >= 0; i--)
            {
                double c = terms[i].Coefficient;

                if (Math.Abs(c) < 1e-12)
                    continue;

                if (!first)
                {
                    Console.Write(c >= 0 ? " + " : " - ");
                }
                else if (c < 0)
                {
                    Console.Write("-");
                }

                double absC = Math.Abs(c);

                if (i == 0)
                {
                    Console.Write($"{absC:0.##}");
                }
                else
                {
                    if (Math.Abs(absC - 1) > 1e-12)
                        Console.Write($"{absC:0.##}");

                    Console.Write("x");

                    if (i > 1)
                        Console.Write($"^{i}");
                }

                first = false;
            }

            if (first)
                Console.Write("0");

            Console.WriteLine();
        }
    }

    // ---------------------------------------------------------
    // BÀI 2.6 (DÃY PHÂN SỐ)
    // ---------------------------------------------------------
    public class FractionList
    {
        private readonly Fraction[] fractions;

        public FractionList(int n)
        {
            if (n < 0)
                throw new ArgumentException("Số phần tử phải >= 0.");

            fractions = new Fraction[n];

            for (int i = 0; i < n; i++)
                fractions[i] = new Fraction();
        }

        public int Length => fractions.Length;

        public Fraction this[int index]
        {
            get => fractions[index];
            set => fractions[index] = value;
        }

        public void Input()
        {
            for (int i = 0; i < fractions.Length; i++)
            {
                Console.WriteLine($"\nPhân số thứ {i + 1}:");
                int numerator = InputHelper.ReadInt("Tử số = ");

                int denominator;
                do
                {
                    denominator = InputHelper.ReadInt("Mẫu số = ");

                    if (denominator == 0)
                        Console.WriteLine("Mẫu số không được bằng 0.");
                }
                while (denominator == 0);

                fractions[i] = new Fraction(numerator, denominator);
            }
        }

        public Fraction Sum()
        {
            Fraction total = new Fraction(0);

            foreach (Fraction f in fractions)
                total += f;

            return total;
        }

        public void Output()
        {
            Console.WriteLine(string.Join(" ", fractions));
        }
    }

    // ---------------------------------------------------------
    // BÀI 2.7 - TÍNH LƯƠNG PHÒNG BAN
    // ---------------------------------------------------------
    public class DepartmentEmployee
    {
        public string FullName { get; set; }
        public decimal BaseSalary { get; set; }
        public int AbsentDays { get; set; }

        public DepartmentEmployee(
            string fullName,
            decimal baseSalary,
            int absentDays)
        {
            FullName = fullName;
            BaseSalary = baseSalary;
            AbsentDays = absentDays;
        }

        public decimal CalculateSalary()
        {
            // Theo đề: mỗi ngày vắng trừ 100.000 VNĐ.
            return BaseSalary - AbsentDays * 100_000m;
        }

        public override string ToString()
        {
            return $"{FullName} - Lương nhận: {CalculateSalary():N0} VNĐ";
        }
    }

    public class Department
    {
        private readonly List<DepartmentEmployee> employees;

        public Department()
        {
            employees = new List<DepartmentEmployee>();
        }

        public void Add(DepartmentEmployee employee)
        {
            employees.Add(employee);
        }

        public void Input()
        {
            int n = InputHelper.ReadInt("Nhập số nhân viên: ", min: 0);

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"\nNhân viên thứ {i + 1}:");

                Console.Write("Họ tên: ");
                string name = Console.ReadLine() ?? "";

                decimal baseSalary =
                    InputHelper.ReadDecimal("Mức lương: ", min: 0);

                int absentDays =
                    InputHelper.ReadInt("Số ngày vắng: ", min: 0);

                Add(new DepartmentEmployee(
                    name,
                    baseSalary,
                    absentDays
                ));
            }
        }

        public decimal TotalSalary()
        {
            decimal total = 0;

            foreach (DepartmentEmployee employee in employees)
                total += employee.CalculateSalary();

            return total;
        }

        public void Output()
        {
            foreach (DepartmentEmployee employee in employees)
                Console.WriteLine(employee);

            Console.WriteLine(
                $"TỔNG LƯƠNG PHÒNG BAN: {TotalSalary():N0} VNĐ"
            );
        }
    }

    // =========================================================
    // PHẦN 3 - KẾ THỪA, ĐA HÌNH, INTERFACE, DELEGATE/EVENT
    // =========================================================

    // ---------------------------------------------------------
    // BÀI 3.1 - Array.Sort(...) với đối tượng
    // ---------------------------------------------------------
    public class SortableStudent : IComparable<SortableStudent>
    {
        public string Name { get; set; }
        public double Score { get; set; }

        public SortableStudent(string name, double score)
        {
            Name = name;
            Score = score;
        }

        public int CompareTo(SortableStudent? other)
        {
            if (other is null)
                return 1;

            // Tăng dần theo điểm.
            return Score.CompareTo(other.Score);
        }

        public override string ToString()
        {
            return $"{Name,-20} {Score:0.##}";
        }
    }

    // ---------------------------------------------------------
    // BÀI 3.2 - SORT MẢNG TỔNG QUÁT BẰNG INTERFACE
    // ---------------------------------------------------------
    public interface IMyComparable<T>
    {
        int CompareTo(T other);
    }

    public static class InterfaceSorter
    {
        public static void Sort<T>(T[] array)
            where T : IMyComparable<T>
        {
            // Selection sort đơn giản để minh họa.
            for (int i = 0; i < array.Length - 1; i++)
            {
                int minIndex = i;

                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j].CompareTo(array[minIndex]) < 0)
                        minIndex = j;
                }

                if (minIndex != i)
                {
                    T temp = array[i];
                    array[i] = array[minIndex];
                    array[minIndex] = temp;
                }
            }
        }
    }

    public class Product : IMyComparable<Product>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public int CompareTo(Product other)
        {
            return Price.CompareTo(other.Price);
        }

        public override string ToString()
        {
            return $"{Name,-15} {Price,12:N0} VNĐ";
        }
    }

    // ---------------------------------------------------------
    // BÀI 3.3 - SORT MẢNG TỔNG QUÁT BẰNG DELEGATE
    // ---------------------------------------------------------
    public delegate int CompareDelegate<T>(T a, T b);

    public static class DelegateSorter
    {
        public static void Sort<T>(
            T[] array,
            CompareDelegate<T> compare)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int selected = i;

                for (int j = i + 1; j < array.Length; j++)
                {
                    if (compare(array[j], array[selected]) < 0)
                        selected = j;
                }

                if (selected != i)
                {
                    T temp = array[i];
                    array[i] = array[selected];
                    array[selected] = temp;
                }
            }
        }
    }

    // ---------------------------------------------------------
    // BÀI 3.4 - CONSOLEMENU + EVENT + KẾ THỪA
    // ---------------------------------------------------------
    public class ConsoleMenu
    {
        protected readonly List<string> items;

        public event Action<int>? Choose;

        public ConsoleMenu()
        {
            items = new List<string>();
        }

        public void AddItem(string item)
        {
            items.Add(item);
        }

        protected virtual void OnChoose(int choice)
        {
            Choose?.Invoke(choice);
        }

        public virtual void Show()
        {
            while (true)
            {
                Console.WriteLine("\n========== MENU ==========");

                for (int i = 0; i < items.Count; i++)
                    Console.WriteLine($"{i + 1}. {items[i]}");

                Console.WriteLine("0. Thoát chương trình");

                int choice = InputHelper.ReadInt("Thực hiện: ");

                if (choice == 0)
                    break;

                if (choice < 1 || choice > items.Count)
                {
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                    continue;
                }

                OnChoose(choice);
            }
        }
    }

    public class QuadraticConsole : ConsoleMenu
    {
        public QuadraticConsole()
        {
            AddItem("Giải phương trình bậc 2");
            Choose += HandleChoose;
        }

        private void HandleChoose(int choice)
        {
            if (choice == 1)
                SolveQuadratic();
        }

        private static void SolveQuadratic()
        {
            Console.WriteLine("\nGiải ax^2 + bx + c = 0");

            double a = InputHelper.ReadDouble("a = ");
            double b = InputHelper.ReadDouble("b = ");
            double c = InputHelper.ReadDouble("c = ");

            const double eps = 1e-12;

            // Nếu a = 0 thì phương trình trở thành bậc nhất.
            if (Math.Abs(a) < eps)
            {
                if (Math.Abs(b) < eps)
                {
                    Console.WriteLine(
                        Math.Abs(c) < eps
                            ? "Phương trình có vô số nghiệm."
                            : "Phương trình vô nghiệm."
                    );
                }
                else
                {
                    Console.WriteLine($"x = {-c / b:0.####}");
                }

                return;
            }

            double delta = b * b - 4 * a * c;

            if (delta < -eps)
            {
                Console.WriteLine("Phương trình vô nghiệm thực.");
            }
            else if (Math.Abs(delta) <= eps)
            {
                double x = -b / (2 * a);
                Console.WriteLine($"Nghiệm kép x = {x:0.####}");
            }
            else
            {
                double sqrtDelta = Math.Sqrt(delta);
                double x1 = (-b + sqrtDelta) / (2 * a);
                double x2 = (-b - sqrtDelta) / (2 * a);

                Console.WriteLine($"x1 = {x1:0.####}");
                Console.WriteLine($"x2 = {x2:0.####}");
            }
        }
    }

    // ---------------------------------------------------------
    // BÀI 3.5 - TÍNH LƯƠNG NHÂN VIÊN
    // Kế thừa + đa hình
    // ---------------------------------------------------------
    public abstract class CompanyEmployee
    {
        public string Id { get; set; }
        public string FullName { get; set; }

        protected CompanyEmployee(string id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }

        public abstract decimal CalculateSalary();

        public virtual void Output()
        {
            Console.WriteLine(
                $"{Id,-8} {FullName,-25} " +
                $"{CalculateSalary(),15:N0} VNĐ"
            );
        }
    }

    public class SalesEmployee : CompanyEmployee
    {
        public decimal BaseSalary { get; set; }
        public int ContractCount { get; set; }

        public SalesEmployee(
            string id,
            string fullName,
            decimal baseSalary,
            int contractCount)
            : base(id, fullName)
        {
            BaseSalary = baseSalary;
            ContractCount = contractCount;
        }

        public override decimal CalculateSalary()
        {
            // Lương cơ bản + 500.000 cho mỗi hợp đồng.
            return BaseSalary + ContractCount * 500_000m;
        }
    }

    public class ProductionEmployee : CompanyEmployee
    {
        public int ProductCount { get; set; }

        public ProductionEmployee(
            string id,
            string fullName,
            int productCount)
            : base(id, fullName)
        {
            ProductCount = productCount;
        }

        public override decimal CalculateSalary()
        {
            // 1.000 VNĐ / sản phẩm.
            decimal salary = ProductCount * 1_000m;

            // Trên 3000 sản phẩm thì thưởng thêm 5%.
            if (ProductCount > 3000)
                salary *= 1.05m;

            return salary;
        }
    }

    public class Company
    {
        private readonly List<CompanyEmployee> employees;

        public Company()
        {
            employees = new List<CompanyEmployee>();
        }

        public void Add(CompanyEmployee employee)
        {
            employees.Add(employee);
        }

        public void Output()
        {
            foreach (CompanyEmployee employee in employees)
                employee.Output();
        }
    }

    // ---------------------------------------------------------
    // BÀI 3.6 - TÍNH ĐIỂM THÍ SINH
    // ---------------------------------------------------------
    public abstract class Candidate
    {
        public string Sbd { get; set; }
        public string FullName { get; set; }
        public double Bai1 { get; set; }
        public double Bai2 { get; set; }
        public double Bai3 { get; set; }

        protected Candidate(
            string sbd,
            string fullName,
            double bai1,
            double bai2,
            double bai3)
        {
            Sbd = sbd;
            FullName = fullName;
            Bai1 = bai1;
            Bai2 = bai2;
            Bai3 = bai3;
        }

        protected double ProgrammingTotal()
        {
            return Bai1 + Bai2 + Bai3;
        }

        public abstract double CalculateTotalScore();

        public virtual void Output()
        {
            Console.WriteLine(
                $"{Sbd,-8} {FullName,-25} " +
                $"Tổng điểm: {CalculateTotalScore():0.##}"
            );
        }
    }

    public class SpecialistCandidate : Candidate
    {
        public double TiengAnh { get; set; }

        public SpecialistCandidate(
            string sbd,
            string fullName,
            double bai1,
            double bai2,
            double bai3,
            double tiengAnh)
            : base(sbd, fullName, bai1, bai2, bai3)
        {
            TiengAnh = tiengAnh;
        }

        private double EnglishBonus()
        {
            // Đề ghi:
            // 7 <= tiếng Anh <= 8 : +1
            // 9 <= tiếng Anh <= 10: +2
            // Với các giá trị nằm giữa 8 và 9, đề không nêu quy tắc.
            if (TiengAnh >= 9 && TiengAnh <= 10)
                return 2;

            if (TiengAnh >= 7 && TiengAnh <= 8)
                return 1;

            return 0;
        }

        public override double CalculateTotalScore()
        {
            return ProgrammingTotal() + EnglishBonus();
        }
    }

    public class SuperCupCandidate : Candidate
    {
        public double Csdl { get; set; }

        public SuperCupCandidate(
            string sbd,
            string fullName,
            double bai1,
            double bai2,
            double bai3,
            double csdl)
            : base(sbd, fullName, bai1, bai2, bai3)
        {
            Csdl = csdl;
        }

        public override double CalculateTotalScore()
        {
            // Tổng điểm của 4 bài thi.
            return ProgrammingTotal() + Csdl;
        }
    }

    public class Contest
    {
        private readonly List<Candidate> candidates;

        public Contest()
        {
            candidates = new List<Candidate>();
        }

        public void Add(Candidate candidate)
        {
            candidates.Add(candidate);
        }

        public void Input()
        {
            int n = InputHelper.ReadInt("Nhập số thí sinh: ", min: 0);

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"\n--- Thí sinh thứ {i + 1} ---");
                Console.WriteLine("1. Chuyên");
                Console.WriteLine("2. Siêu cúp");

                int type;
                do
                {
                    type = InputHelper.ReadInt("Loại thí sinh: ");
                }
                while (type != 1 && type != 2);

                Console.Write("SBD: ");
                string sbd = Console.ReadLine() ?? "";

                Console.Write("Họ tên: ");
                string name = Console.ReadLine() ?? "";

                double b1 = ReadScore("Bài 1: ");
                double b2 = ReadScore("Bài 2: ");
                double b3 = ReadScore("Bài 3: ");

                if (type == 1)
                {
                    double english = ReadScore("Tiếng Anh: ");

                    Add(new SpecialistCandidate(
                        sbd, name, b1, b2, b3, english
                    ));
                }
                else
                {
                    double database = ReadScore("CSDL: ");

                    Add(new SuperCupCandidate(
                        sbd, name, b1, b2, b3, database
                    ));
                }
            }
        }

        private static double ReadScore(string prompt)
        {
            return InputHelper.ReadDouble(prompt, min: 0, max: 10);
        }

        public void Output()
        {
            foreach (Candidate candidate in candidates)
                candidate.Output();
        }
    }

    // =========================================================
    // HÀM NHẬP DỮ LIỆU DÙNG CHUNG
    // =========================================================
    public static class InputHelper
    {
        public static int ReadInt(
            string prompt,
            int? min = null,
            int? max = null)
        {
            while (true)
            {
                Console.Write(prompt);

                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    if (min.HasValue && value < min.Value)
                    {
                        Console.WriteLine(
                            $"Giá trị phải >= {min.Value}."
                        );
                        continue;
                    }

                    if (max.HasValue && value > max.Value)
                    {
                        Console.WriteLine(
                            $"Giá trị phải <= {max.Value}."
                        );
                        continue;
                    }

                    return value;
                }

                Console.WriteLine("Dữ liệu không hợp lệ. Hãy nhập số nguyên.");
            }
        }

        public static double ReadDouble(
            string prompt,
            double? min = null,
            double? max = null)
        {
            while (true)
            {
                Console.Write(prompt);

                if (double.TryParse(Console.ReadLine(), out double value))
                {
                    if (min.HasValue && value < min.Value)
                    {
                        Console.WriteLine(
                            $"Giá trị phải >= {min.Value}."
                        );
                        continue;
                    }

                    if (max.HasValue && value > max.Value)
                    {
                        Console.WriteLine(
                            $"Giá trị phải <= {max.Value}."
                        );
                        continue;
                    }

                    return value;
                }

                Console.WriteLine("Dữ liệu không hợp lệ. Hãy nhập số.");
            }
        }

        public static decimal ReadDecimal(
            string prompt,
            decimal? min = null,
            decimal? max = null)
        {
            while (true)
            {
                Console.Write(prompt);

                if (decimal.TryParse(Console.ReadLine(), out decimal value))
                {
                    if (min.HasValue && value < min.Value)
                    {
                        Console.WriteLine(
                            $"Giá trị phải >= {min.Value}."
                        );
                        continue;
                    }

                    if (max.HasValue && value > max.Value)
                    {
                        Console.WriteLine(
                            $"Giá trị phải <= {max.Value}."
                        );
                        continue;
                    }

                    return value;
                }

                Console.WriteLine("Dữ liệu không hợp lệ. Hãy nhập số.");
            }
        }
    }

    // =========================================================
    // PROGRAM - MENU CHẠY THỬ TẤT CẢ BÀI
    // =========================================================
    public static class Program
    {
        public static void Main()
        {
            // Thiết lập UTF-8 để nhập/xuất tiếng Việt có dấu chính xác trên Terminal.
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                PrintMainMenu();

                int choice = InputHelper.ReadInt("Chọn bài: ");

                Console.WriteLine();

                switch (choice)
                {
                    case 0:
                        Console.WriteLine("Kết thúc chương trình.");
                        return;

                    case 11:
                        Demo11();
                        break;

                    case 12:
                        Demo12();
                        break;

                    case 13:
                        Demo13();
                        break;

                    case 14:
                        Demo14();
                        break;

                    case 15:
                        Demo15();
                        break;

                    case 21:
                        Demo21();
                        break;

                    case 22:
                        Demo22();
                        break;

                    case 23:
                        Demo23Array();
                        break;

                    case 24:
                        Demo24Matrix();
                        break;

                    case 25:
                        Demo25Polynomial();
                        break;

                    case 26:
                        Demo26FractionList();
                        break;

                    case 27:
                        Demo27();
                        break;

                    case 31:
                        Demo31();
                        break;

                    case 32:
                        Demo32();
                        break;

                    case 33:
                        Demo33();
                        break;

                    case 34:
                        Demo34();
                        break;

                    case 35:
                        Demo35();
                        break;

                    case 36:
                        Demo36();
                        break;

                    default:
                        Console.WriteLine("Không có bài này.");
                        break;
                }

                Pause();
            }
        }

        private static void PrintMainMenu()
        {
            Console.Clear();

            Console.WriteLine("==================================================");
            Console.WriteLine(" THỰC HÀNH 02 - LẬP TRÌNH HƯỚNG ĐỐI TƯỢNG C#");
            Console.WriteLine("==================================================");
            Console.WriteLine("11  - Bài 1.1: Tính tuổi sinh viên");
            Console.WriteLine("12  - Bài 1.2: Point");
            Console.WriteLine("13  - Bài 1.3: Person");
            Console.WriteLine("14  - Bài 1.4: Phân số");
            Console.WriteLine("15  - Bài 1.5: Đơn thức");
            Console.WriteLine();
            Console.WriteLine("21  - Bài 2.1: ArrayPoint");
            Console.WriteLine("22  - Bài 2.2: PersonList");
            Console.WriteLine("23  - Bài 2.3: Mảng 1 chiều");
            Console.WriteLine("24  - Bài 2.4: Mảng 2 chiều");
            Console.WriteLine("25  - Bài 2.5: Đa thức");
            Console.WriteLine("26  - Bài 2.6: Dãy phân số");
            Console.WriteLine("27  - Bài 2.7: Lương phòng ban");
            Console.WriteLine();
            Console.WriteLine("31  - Bài 3.1: Array.Sort");
            Console.WriteLine("32  - Bài 3.2: Sort bằng interface");
            Console.WriteLine("33  - Bài 3.3: Sort bằng delegate");
            Console.WriteLine("34  - Bài 3.4: ConsoleMenu/Event");
            Console.WriteLine("35  - Bài 3.5: Lương nhân viên");
            Console.WriteLine("36  - Bài 3.6: Điểm thí sinh");
            Console.WriteLine("0   - Thoát");
            Console.WriteLine("==================================================");
        }

        private static void Pause()
        {
            Console.WriteLine("\nNhấn Enter để quay lại menu...");
            Console.ReadLine();
        }

        // ----------------- DEMO 1.1 -----------------
        private static void Demo11()
        {
            Console.WriteLine("=== BÀI 1.1 ===");

            StudentAge student = new StudentAge();
            student.Input();

            Console.WriteLine();
            student.Output();
        }

        // ----------------- DEMO 1.2 -----------------
        private static void Demo12()
        {
            Console.WriteLine("=== BÀI 1.2 ===");

            Point a = new Point();
            Point b = new Point();

            Console.WriteLine("Nhập điểm A:");
            a.Input();

            Console.WriteLine("Nhập điểm B:");
            b.Input();

            Console.WriteLine($"\nA = {a}");
            Console.WriteLine($"B = {b}");
            Console.WriteLine($"A + B = {a + b}");
            Console.WriteLine($"A - B = {a - b}");
            Console.WriteLine($"-A = {-a}");

            Console.WriteLine(
                $"Khoảng cách (thành viên) = {a.DistanceTo(b):0.####}"
            );

            Console.WriteLine(
                $"Khoảng cách (tĩnh) = {Point.Distance(a, b):0.####}"
            );

            Console.WriteLine(
                $"Trung điểm (thành viên) = {a.MidPoint(b)}"
            );

            Console.WriteLine(
                $"Trung điểm (tĩnh) = {Point.MidPoint(a, b)}"
            );
        }

        // ----------------- DEMO 1.3 -----------------
        private static void Demo13()
        {
            Console.WriteLine("=== BÀI 1.3 ===");

            Person person = new Person();
            person.Input();

            Console.WriteLine();
            person.Output();

            Person copy = new Person(person);
            Console.WriteLine("\nBản sao:");
            copy.Output();
        }

        // ----------------- DEMO 1.4 -----------------
        private static void Demo14()
        {
            Console.WriteLine("=== BÀI 1.4 ===");

            Fraction a = ReadFraction("A");
            Fraction b = ReadFraction("B");

            Console.WriteLine($"\nA = {a}");
            Console.WriteLine($"B = {b}");
            Console.WriteLine($"+A = {+a}");
            Console.WriteLine($"-A = {-a}");
            Console.WriteLine($"A + B = {a + b}");
            Console.WriteLine($"A - B = {a - b}");
            Console.WriteLine($"A * B = {a * b}");

            if (b.Numerator != 0)
                Console.WriteLine($"A / B = {a / b}");
            else
                Console.WriteLine("A / B: không thể chia cho 0.");

            Console.WriteLine($"A > B  : {a > b}");
            Console.WriteLine($"A < B  : {a < b}");
            Console.WriteLine($"A >= B : {a >= b}");
            Console.WriteLine($"A <= B : {a <= b}");
            Console.WriteLine($"A == B : {a == b}");
            Console.WriteLine($"A != B : {a != b}");
        }

        private static Fraction ReadFraction(string name)
        {
            Console.WriteLine($"Nhập phân số {name}:");

            int numerator = InputHelper.ReadInt("Tử số = ");
            int denominator;

            do
            {
                denominator = InputHelper.ReadInt("Mẫu số = ");

                if (denominator == 0)
                    Console.WriteLine("Mẫu số không được bằng 0.");
            }
            while (denominator == 0);

            return new Fraction(numerator, denominator);
        }

        // ----------------- DEMO 1.5 -----------------
        private static void Demo15()
        {
            Console.WriteLine("=== BÀI 1.5 ===");

            double a = InputHelper.ReadDouble("Nhập hệ số a: ");
            int n = InputHelper.ReadInt("Nhập số mũ n: ", min: 0);

            Monomial p = new Monomial(a, n);

            double x = InputHelper.ReadDouble("Nhập x: ");

            Console.WriteLine($"P(x) = {p}");
            Console.WriteLine($"P({x}) = {p.Evaluate(x):0.####}");
            Console.WriteLine($"P'(x) = {p.Derivative()}");
        }

        // ----------------- DEMO 2.1 -----------------
        private static void Demo21()
        {
            Console.WriteLine("=== BÀI 2.1 ===");

            ArrayPoint array = new ArrayPoint();
            array.Input();

            Console.WriteLine("\nDanh sách điểm:");
            array.Output();

            if (array.Count > 0)
            {
                Console.WriteLine(
                    $"\nTruy cập bằng indexer: array[0] = {array[0]}"
                );
            }
        }

        // ----------------- DEMO 2.2 -----------------
        private static void Demo22()
        {
            Console.WriteLine("=== BÀI 2.2 ===");

            PersonList list = new PersonList();
            list.Input();

            Console.WriteLine("\nTẤT CẢ:");
            list.Output();

            Console.WriteLine("\nNGƯỜI CÒN SỐNG:");
            PersonList living = list.LivingPeople();
            living.Output();
        }

        // ----------------- DEMO 2.3 MẢNG 1 CHIỀU -----------------
        private static void Demo23Array()
        {
            Console.WriteLine("=== BÀI 2.3 - MẢNG 1 CHIỀU ===");

            int n = InputHelper.ReadInt("Nhập n: ", min: 0);

            IntArray1D array = new IntArray1D(n);
            array.Input();

            Console.WriteLine("\nDãy vừa nhập:");
            array.Output();

            List<int> evens = array.FindEvenNumbers();

            Console.WriteLine(
                evens.Count == 0
                    ? "Không có số chẵn."
                    : $"Các số chẵn: {string.Join(" ", evens)}"
            );
        }

        // ----------------- DEMO 2.4 MẢNG 2 CHIỀU -----------------
        private static void Demo24Matrix()
        {
            Console.WriteLine("=== BÀI 2.4 - MẢNG 2 CHIỀU ===");

            int n = InputHelper.ReadInt("Nhập số dòng n: ", min: 0);
            int m = InputHelper.ReadInt("Nhập số cột m: ", min: 0);

            IntMatrix2D matrix = new IntMatrix2D(n, m);
            matrix.Input();

            Console.WriteLine("\nMa trận:");
            matrix.Output();

            List<int> primes = matrix.FindPrimes();

            Console.WriteLine(
                primes.Count == 0
                    ? "Không có số nguyên tố."
                    : $"Các số nguyên tố: {string.Join(" ", primes)}"
            );
        }

        // ----------------- DEMO 2.5 ĐA THỨC -----------------
        private static void Demo25Polynomial()
        {
            Console.WriteLine("=== BÀI 2.5 - ĐA THỨC ===");

            int degree = InputHelper.ReadInt(
                "Nhập bậc đa thức n: ",
                min: 0
            );

            Polynomial polynomial = new Polynomial(degree);
            polynomial.Input();

            Console.Write("\nP(x) = ");
            polynomial.Output();

            double x = InputHelper.ReadDouble("Nhập x: ");

            Console.WriteLine(
                $"P({x}) = {polynomial.Evaluate(x):0.####}"
            );
        }

        // ----------------- DEMO 2.6 DÃY PHÂN SỐ -----------------
        private static void Demo26FractionList()
        {
            Console.WriteLine("=== BÀI 2.6 - DÃY PHÂN SỐ ===");

            int n = InputHelper.ReadInt(
                "Nhập số phân số: ",
                min: 0
            );

            FractionList list = new FractionList(n);
            list.Input();

            Console.WriteLine("\nDãy phân số:");
            list.Output();

            Console.WriteLine($"Tổng = {list.Sum()}");
        }

        // ----------------- DEMO 2.7 -----------------
        private static void Demo27()
        {
            Console.WriteLine("=== BÀI 2.7 - TÍNH LƯƠNG PHÒNG BAN ===");

            Department department = new Department();
            department.Input();

            Console.WriteLine("\nBẢNG LƯƠNG:");
            department.Output();
        }

        // ----------------- DEMO 3.1 -----------------
        private static void Demo31()
        {
            Console.WriteLine("=== BÀI 3.1 ===");

            SortableStudent[] students =
            {
                new SortableStudent("An", 7.5),
                new SortableStudent("Bình", 9.0),
                new SortableStudent("Cường", 6.5),
                new SortableStudent("Dung", 8.0)
            };

            Console.WriteLine("Trước khi sắp xếp:");
            foreach (SortableStudent s in students)
                Console.WriteLine(s);

            Array.Sort(students);

            Console.WriteLine("\nSau Array.Sort (tăng dần theo điểm):");
            foreach (SortableStudent s in students)
                Console.WriteLine(s);
        }

        // ----------------- DEMO 3.2 -----------------
        private static void Demo32()
        {
            Console.WriteLine("=== BÀI 3.2 ===");

            Product[] products =
            {
                new Product("Bàn phím", 350_000),
                new Product("Chuột", 200_000),
                new Product("Màn hình", 3_000_000),
                new Product("Tai nghe", 500_000)
            };

            Console.WriteLine("Trước sắp xếp:");
            foreach (Product p in products)
                Console.WriteLine(p);

            InterfaceSorter.Sort(products);

            Console.WriteLine("\nSau sắp xếp tăng dần theo giá:");
            foreach (Product p in products)
                Console.WriteLine(p);
        }

        // ----------------- DEMO 3.3 -----------------
        private static void Demo33()
        {
            Console.WriteLine("=== BÀI 3.3 ===");

            int[] numbers = { 5, 1, 8, 3, 2, 10, -1 };

            Console.WriteLine(
                $"Ban đầu: {string.Join(" ", numbers)}"
            );

            DelegateSorter.Sort(
                numbers,
                (a, b) => a.CompareTo(b)
            );

            Console.WriteLine(
                $"Tăng dần: {string.Join(" ", numbers)}"
            );

            DelegateSorter.Sort(
                numbers,
                (a, b) => b.CompareTo(a)
            );

            Console.WriteLine(
                $"Giảm dần: {string.Join(" ", numbers)}"
            );
        }

        // ----------------- DEMO 3.4 -----------------
        private static void Demo34()
        {
            Console.WriteLine("=== BÀI 3.4 ===");

            QuadraticConsole app = new QuadraticConsole();
            app.Show();
        }

        // ----------------- DEMO 3.5 -----------------
        private static void Demo35()
        {
            Console.WriteLine("=== BÀI 3.5 ===");

            Company company = new Company();

            company.Add(
                new SalesEmployee(
                    "KD01",
                    "Nguyễn Văn A",
                    8_000_000m,
                    4
                )
            );

            company.Add(
                new SalesEmployee(
                    "KD02",
                    "Trần Thị B",
                    9_000_000m,
                    2
                )
            );

            company.Add(
                new ProductionEmployee(
                    "SX01",
                    "Lê Văn C",
                    2500
                )
            );

            company.Add(
                new ProductionEmployee(
                    "SX02",
                    "Phạm Thị D",
                    3500
                )
            );

            Console.WriteLine("Mã       Họ tên                           Lương");
            company.Output();
        }

        // ----------------- DEMO 3.6 -----------------
        private static void Demo36()
        {
            Console.WriteLine("=== BÀI 3.6 ===");

            Contest contest = new Contest();
            contest.Input();

            Console.WriteLine("\nKẾT QUẢ:");
            contest.Output();
        }
    }
}

