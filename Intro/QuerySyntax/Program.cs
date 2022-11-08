namespace Masterlinq
{
    public class ChessPlayer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }
        public int Rating { get; set; }
        public string Country { get; set; }
        public int Id { get; set; }

        public override string ToString()
        {
            return $"Full Name: {FirstName} {LastName}, Rating = {Rating}, from {Country}, born in {BirthYear}";
        }

        public static ChessPlayer parseFromCsv(string line)
        {
            string[] parts = line.Split(";");
            return new ChessPlayer()
            {
                Id = int.Parse(parts[0]),
                LastName = parts[1].Split(",")[0].Trim(),
                FirstName = parts[1].Split(",")[1].Trim(),
                Country = parts[3],
                Rating = int.Parse(parts[4]),
                BirthYear = int.Parse(parts[6]),
            };
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var fileLocations = Path.Combine(Directory.GetCurrentDirectory(), "ChessStats", "Top100ChessPlayers.csv");
            ParseCsv(fileLocations);
        }

        private static void ParseCsv(string file)
        {
            var list = File.ReadAllLines(file)
                .Skip(1)
                .Select(ChessPlayer.parseFromCsv)
                .Where(player => player.BirthYear > 1988)
                .OrderByDescending(player => player.Rating)
                .Take(10);

            var listFiltered = from player in list
                where player.BirthYear > 1_988
                orderby player.Rating descending
                select player;

            foreach (var chessPlayer in list)
            {
                Console.WriteLine(chessPlayer);
            }
        }

        private static void NamedNonSeparate()
        {
        }

        private static bool GetRatingsOver2700(int arg)
        {
            return arg > 2_700;
        }
    }
}