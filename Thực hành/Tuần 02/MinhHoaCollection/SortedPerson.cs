namespace MinhHoaCollection
{
    public static class SortedPerson
    {
        public class Person
        {
            public string Name { get; set; }
            public string Location { get; set; }
            public int Age { get; set; }

            public Person(
                string name,
                string location,
                int age)
            {
                Name = name;
                Location = location;
                Age = age;
            }

            public override string ToString()
            {
                return $"{Name,-20} " +
                       $"Tuổi: {Age,-3} " +
                       $"Nơi ở: {Location}";
            }
        }

        public class PersonComparer
            : IComparer<Person>
        {
            public int Compare(
                Person? x,
                Person? y)
            {
                if (x == null && y == null)
                    return 0;

                if (x == null)
                    return -1;

                if (y == null)
                    return 1;

                // 1. So sánh theo tuổi
                int result =
                    x.Age.CompareTo(y.Age);

                // 2. Nếu bằng tuổi thì theo tên
                if (result == 0)
                {
                    result =
                        string.Compare(
                            x.Name,
                            y.Name,
                            StringComparison.OrdinalIgnoreCase
                        );
                }

                // 3. Nếu tên cũng giống thì theo nơi ở
                if (result == 0)
                {
                    result =
                        string.Compare(
                            x.Location,
                            y.Location,
                            StringComparison.OrdinalIgnoreCase
                        );
                }

                return result;
            }
        }

        public static void Run()
        {
            Console.WriteLine(
                "===== DEMO SORTEDSET + ICOMPARER ====="
            );

            SortedSet<Person> people =
                new SortedSet<Person>(
                    new PersonComparer()
                );

            people.Add(
                new Person(
                    "An",
                    "TP.HCM",
                    22
                )
            );

            people.Add(
                new Person(
                    "Bình",
                    "Đà Nẵng",
                    19
                )
            );

            people.Add(
                new Person(
                    "Cường",
                    "Hà Nội",
                    25
                )
            );

            people.Add(
                new Person(
                    "Dũng",
                    "TP.HCM",
                    19
                )
            );

            people.Add(
                new Person(
                    "An",
                    "Hà Nội",
                    22
                )
            );

            Console.WriteLine(
                "\nDanh sách đã sắp xếp:"
            );

            foreach (Person person in people)
            {
                Console.WriteLine(person);
            }
        }
    }
}