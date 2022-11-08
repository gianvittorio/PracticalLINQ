namespace MasterLinq
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var ratings = new List<int>()
            {
                2_200,
                2_400,
                2_800,
                2_820
            };

            var ratingsAbove2700Lambda = ratings.Where(r => r > 2_700);
            foreach (var i in ratingsAbove2700Lambda)
            {
                Console.WriteLine(i);
            }

            var ratingsAbove2700Named = ratings.Where(GetRatingsOver2700);
            foreach (var i in ratingsAbove2700Named)
            {
                Console.WriteLine(i);
            }

            var ratingsAbove2700Anonymous = ratings.Where(delegate(int rating) { return rating > 2_7000; });
            foreach (var i in ratingsAbove2700Anonymous)
            {
                Console.WriteLine();(i);
            }
        }

        private static bool GetRatingsOver2700(int rating)
        {
            return rating > 2_700;
        }
    }
}