using System.Collections.Generic;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Attempts to determine the number of elements in a sequence without forcing an enumeration.
        /// </summary>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">
        /// When this method returns, contains the number of elements in source,
        /// or 0 if the count couldn't be determined without enumeration.
        /// </param>
        /// <returns>true if the count of source can be determined without enumeration; otherwise, false.</returns>
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
