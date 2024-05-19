namespace ByReplace.Test.Common.Asserts;

public static class AssertBr
{
    public static void Single<T>(IEnumerable<T> collection, Action<T> action)
    {
        if (collection is null || collection == Enumerable.Empty<T>())
        {
            throw new ArgumentNullException("The collection is empty");
        }

        var first = collection.FirstOrDefault();

        action.Invoke(first!);
    }
}