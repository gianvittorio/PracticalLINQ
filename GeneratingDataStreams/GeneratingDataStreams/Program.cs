namespace MasterLinq
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Generating Range:");
            foreach (var num in Enumerable.Range(5, 8))
            {
                Console.WriteLine($"Num: {num}");
            }

            Console.WriteLine("Repeating:");
            foreach (var num in Enumerable.Repeat(10, 5))
            {
                Console.WriteLine($"Num: {num}");
            }
            
            Console.WriteLine("Randomizing");
            foreach (var d in RandomStream.Generate().Where(num => num > .7f).Take(5))
            {
                Console.WriteLine($"Num: {d.ToString("F2")}");
            }

            Console.ReadKey();
        }

        public static IEnumerable<int> GetData()
        {
            return Enumerable.Empty<int>();
        }
    }

    public static class RandomStream
    {
        public static IEnumerable<double> Generate()
        {
            var random = new Random();
            for (;;)
            {
                yield return random.NextDouble();
            }
        }
    }
}