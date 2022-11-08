namespace Server.Tools;

internal static class EnumerableExtensions
{
    internal static T RandomElement<T> (this IEnumerable<T> enumerable)
    {
        var index = Random.Shared.Next(0, enumerable.Count());
        return enumerable.ElementAt(index);
    }
}