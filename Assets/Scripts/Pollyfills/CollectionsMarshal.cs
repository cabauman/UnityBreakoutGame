using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices
{
    [ExcludeFromCodeCoverage]
    public static class CollectionsMarshal
    {
        class ListDummy<T>
        {
            public T[] Items;
            int size;
            int version;
        }

        public static Span<T> AsSpan<T>(List<T> list)
        {
            return Unsafe.As<ListDummy<T>>(list).Items.AsSpan(0, list.Count);
        }
    }
}
