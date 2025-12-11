using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#nullable enable

namespace GameCtor.DevToolbox
{
    public static class Ensure
    {
        public static void NotNull<T>([NotNull] T? obj, [CallerArgumentExpression("obj")] string paramName = "")
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
