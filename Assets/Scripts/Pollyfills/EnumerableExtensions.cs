using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq
{
    [ExcludeFromCodeCoverage]
    public static class EnumerableExtensions
    {
        public static bool TryGetNonEnumeratedCount<T>(this IEnumerable<T> source, out int count)
        {
            if (source is ICollection<T> collection)
            {
                count = collection.Count;
                return true;
            }

            if (source is IReadOnlyCollection<T> readOnlyCollection)
            {
                count = readOnlyCollection.Count;
                return true;
            }

            count = 0;
            return false;
        }
    }
}
