namespace FunctionalProgramming.Extending
{
    public static class DateTimeExtensions
    {
        public static string ToDeviceFormat(this DateTime dt)
        {
            string result = dt.ToString("yyyyMMddhhmmss");
            return dt.Year >= 2000 ? result : result.Substring(2);
        }
    }
}