namespace Masterlinq;

public static class LinqExtensions
{
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
    {
        //var result = new List<T>();
        foreach (var item in collection)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }

        //return result;
    } 
}