using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices
{
    public static class CollectionsMarshal
    {
        /// <summary>
        /// Gets a Span<T> view over the data in a list.
        /// Items should not be added or removed from the List<T> while the Span<T> is in use.
        /// </summary>
        public static Span<T> AsSpan<T>(List<T> list)
        {
            return Unsafe.As<ListDummy<T>>(list).Items.AsSpan(0, list.Count);
        }

        private class ListDummy<T>
        {
            public T[] Items;
            private int _size;
            private int _version;
        }
    }
}
