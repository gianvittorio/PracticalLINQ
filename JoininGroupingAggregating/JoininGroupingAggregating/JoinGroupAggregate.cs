using Microsoft.VisualBasic;

namespace MasterLinq
{
    public class JoinGroupAggregate
    {
        public static void Demo()
        {
            var fileLocation = Path.Combine(Directory.GetCurrentDirectory(), "ChessStats", "Top100ChessPlayers.csv");
            //JoinDemo(fileLocation);
            //GroupByDemo(fileLocation);
            //GroupJoinDemo();
            //ZipDemo();
            //MinMaxSumAvg(fileLocation);
            //ConcatUnionDemo();
            IntersectExceptDemo();
        }

        public static void IntersectExceptDemo()
        {
            var p1 = new List<string>() { "milk", "butter", "soda" };
            var p2 = new List<string>() { "coffee", "Butter", "milk", "pizza" };

            var intersect = p1.Intersect(p2, new ProductsComparer());
            var except = p1.Except(p2, new ProductsComparer());

            foreach (var product in intersect)
            {
                Console.Write(product + ",");
            }
            Console.WriteLine();
            
            foreach (var product in except)
            {
                Console.Write(product + ",");
            }
            Console.WriteLine();
        }

        public class ProductsComparer : IEqualityComparer<string>
        {
            public bool Equals(string? x, string? y)
            {
                return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(string obj)
            {
                return obj.ToUpper().GetHashCode();
            }
        }

        public static void ConcatUnionDemo()
        {
            var p1 = new List<string>() { "milk", "butter", "soda" };
            var p2 = new List<string>() { "coffee", "butter", "milk", "pizza" };

            Console.WriteLine("Concat");
            foreach (var product in p1.Concat(p2))
            {
                Console.Write($"{product}, ");
            }
            Console.WriteLine();

            Console.WriteLine("Union");
            foreach (var product in p1.Union(p2))
            {
                Console.Write($"{product}, ");
            }
            Console.WriteLine();

            Console.WriteLine("Custom Union");
            foreach (var product in p1.Union(p2, new ProductsComparer()))
            {
                Console.Write($"{product}, ");
            }
        }

        public static void MinMaxSumAvg(string file)
        {
            var players = File.ReadAllLines(file)
                .Skip(1)
                .Select(ChessPlayer.ParseFileCsv)
                .Where(player => player.BirthYear > 1988)
                .Take(10)
                .OrderByDescending(p => p.Rating)
                .ToList();

            Console.WriteLine($"The lowest rating in Top 10: {players.Min(x => x.Rating)}");
            Console.WriteLine($"The highest rating in Top 10: {players.Max(x => x.Rating)}");
            Console.WriteLine($"The average rating in Top 10: {players.Average(x => x.Rating)}");
            Console.WriteLine($"The sum rating in Top 10: {players.Sum(x => x.Rating)}");
        }

        public static void ZipDemo()
        {
            List<Team> teams = new List<Team>()
            {
                new Team() { Name = "Bavaria", Country = "Germany" },
                new Team() { Name = "Barcelona", Country = "Spain" },
                new Team() { Name = "Juventus", Country = "Italy" }
            };
            List<Player> players = new List<Player>()
            {
                new Player() { Name = "Messi", Team = "Barcelona" },
                new Player() { Name = "Neymar", Team = "Barcelona" },
                new Player() { Name = "Robben", Team = "Bavaria" },
                new Player() { Name = "Buffon", Team = "Juventus" }
            };

            var result = players.Zip(
                teams,
                (player, team) => new
                {
                    Name = player.Name,
                    Team = team.Name,
                    Country = team.Country
                }
            );

            foreach (var item in result)
            {
                Console.WriteLine($"{item.Name} - {item.Team} - {item.Country}");
            }
        }

        public static void GroupByDemo(string file)
        {
            var players = File.ReadAllLines(file)
                .Skip(1)
                .Select(ChessPlayer.ParseFileCsv)
                .Where(player => player.BirthYear > 1988)
                .Take(10)
                .GroupBy(p => p.Country)
                .OrderByDescending(g => g.Key)
                .ToList();

            foreach (var player in players)
            {
                Console.WriteLine($"\nThe following players live in {player.Key}");
                foreach (var p in player)
                {
                    Console.WriteLine($"Name: {p.LastName}, Rating: {p.Rating}");
                }
            }
        }

        public static void GroupJoinDemo()
        {
            List<Team> teams = new List<Team>()
            {
                new Team() { Name = "Bavaria", Country = "Germany" },
                new Team() { Name = "Barcelona", Country = "Spain" },
                new Team() { Name = "Juventus", Country = "Italy" }
            };
            List<Player> players = new List<Player>()
            {
                new Player() { Name = "Messi", Team = "Barcelona" },
                new Player() { Name = "Neymar", Team = "Barcelona" },
                new Player() { Name = "Robben", Team = "Bavaria" },
                new Player() { Name = "Buffon", Team = "Juventus" }
            };

            var result = teams.GroupJoin(
                players,
                t => t.Name,
                pl => pl.Team,
                (team, pls) => new
                {
                    Name = team.Name,
                    Country = team.Country,
                    Players = players.Select(p => p.Name)
                }
            );
            foreach (var team in result)
            {
                Console.WriteLine($"Players in {team.Name}");
                foreach (var player in team.Players)
                {
                    Console.WriteLine(player);
                }

                Console.WriteLine();
            }
        }

        public static void JoinDemo(string file)
        {
            var players = File.ReadAllLines(file)
                .Skip(1)
                .Select(ChessPlayer.ParseFileCsv)
                .Where(player => player.BirthYear > 1988)
                .Take(10)
                .ToList();

            var tournaments = Tournament.GetDemoStats();

            var join = players.Join(
                tournaments,
                p => p.Id,
                t => t.PlayerId,
                (p, t) => new
                {
                    p.LastName,
                    p.Rating,
                    t.Title,
                    t.TakenPlace,
                    t.Country
                }
            );

            foreach (var item in join)
            {
                Console.WriteLine(
                    $"{item.LastName} took {item.TakenPlace} place at {item.Title}. Has rating {item.Rating}");
            }

            var selectMany = join.GroupBy(x => x.Country)
                .SelectMany(g => g.OrderBy(grp => grp.TakenPlace));

            foreach (var item in selectMany)
            {
                Console.WriteLine(
                    $"{item.LastName} took {item.TakenPlace} place at {item.Title}. Has rating {item.Rating}");
            }
        }
    }

    public class Tournament
    {
        public int PlayerId { get; set; }
        public string Title { get; set; }
        public int TakenPlace { get; set; }
        public string Country { get; set; }

        public static IEnumerable<Tournament> GetDemoStats()
        {
            return new List<Tournament>()
            {
                new Tournament() { Country = "Germany", PlayerId = 1, TakenPlace = 1, Title = "Tournament 1" },
                new Tournament() { Country = "USA", PlayerId = 1, TakenPlace = 3, Title = "Tournament 2" },
                new Tournament() { Country = "Russia", PlayerId = 1, TakenPlace = 2, Title = "Tournament 1" },
                new Tournament() { Country = "Germany", PlayerId = 2, TakenPlace = 2, Title = "Tournament 1" },
                new Tournament() { Country = "USA", PlayerId = 2, TakenPlace = 1, Title = "Tournament 1" },
                new Tournament() { Country = "Russia", PlayerId = 2, TakenPlace = 1, Title = "Tournament 1" }
            };
        }
    }

    public class Team
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }

    public class Player
    {
        public string Name { get; set; }
        public string Team { get; set; }
    }
}