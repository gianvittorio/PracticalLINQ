using FunctionalProgramming.Extending;

namespace FunctionalProgramming
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dt1 = new DateTime(2_000, 12, 20, 01, 02, 03);
            var result1 = dt1.ToDeviceFormat();
            Console.WriteLine(result1);

            var dt2 = new DateTime(1_999, 12, 20, 01, 02, 03);
            var result2 = dt2.ToDeviceFormat();
            Console.WriteLine(result2);
            
            Console.ReadLine();
        }
    }
}