using System.Text.RegularExpressions;

namespace MinhHoaCollection
{
    public static class DemTuTrongChuoi
    {
        public static Dictionary<string, int> CountWords(string text)
        {
            Dictionary<string, int> frequencies =
                new Dictionary<string, int>();

            string[] words = Regex.Split(
                text.ToLower(),
                @"\W+"
            );

            foreach (string word in words)
            {
                if (string.IsNullOrWhiteSpace(word))
                    continue;

                if (frequencies.ContainsKey(word))
                {
                    frequencies[word]++;
                }
                else
                {
                    frequencies[word] = 1;
                }
            }

            return frequencies;
        }

        public static void Run()
        {
            Console.WriteLine(
                "===== DEMO DICTIONARY: ĐẾM TỪ ====="
            );

            Console.Write("Nhập chuỗi: ");
            string text = Console.ReadLine() ?? "";

            Dictionary<string, int> result =
                CountWords(text);

            Console.WriteLine("\nKết quả:");

            foreach (KeyValuePair<string, int> item in result)
            {
                Console.WriteLine(
                    $"{item.Key}: {item.Value}"
                );
            }
        }
    }
}