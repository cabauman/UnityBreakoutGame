using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Profiling;

#nullable enable

namespace GameCtor.DevToolbox
{
    public interface ISpanFormattable : IFormattable
    {
        public bool TryFormat(
            Span<char> destination,
            out int charsWritten,
            ReadOnlySpan<char> format,
            IFormatProvider? provider);
    }

    public struct MyStruct : ISpanFormattable
    {
        public float X;
        public float Y;

        public string ToString(string format, IFormatProvider provider)
        {
            // First, format into a temporary buffer to measure length
            Span<char> temp = stackalloc char[64];
            if (!TryFormat(temp, out int written, format.AsSpan(), provider))
            {
                return $"{X}, {Y}";
            }

            // Now create the string with exact length
            return string.Create(written, this, (span, point) =>
            {
                point.TryFormat(span, out _, format.AsSpan(), provider);
            });
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            charsWritten = 0;

            // Format X
            if (!X.TryFormat(destination, out int writtenX, format, provider))
            {
                return false;
            }

            // Add separator
            if (writtenX >= destination.Length)
            {
                return false;
            }

            destination[writtenX] = ',';
            writtenX++;

            if (writtenX >= destination.Length)
            {
                return false;
            }

            destination[writtenX] = ' ';
            writtenX++;

            // Format Y
            if (!Y.TryFormat(destination.Slice(writtenX), out int writtenY, format, provider))
            {
                return false;
            }

            charsWritten = writtenX + writtenY;
            return true;
        }
    }

    [InterpolatedStringHandler]
    public readonly ref struct InterpolatedStringHandler
    {
        [ThreadStatic]
        private static StringBuilder _sb = new(256);
        public InterpolatedStringHandler(
            int literalLength,
            int formattedCount,
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            _sb.Clear();
            ReadOnlySpan<char> fileName = System.IO.Path.GetFileName(callerFilePath.AsSpan());
            _sb
                .Append('[')
                .Append(fileName)
                .Append(':')
                .Append(callerMemberName)
                .Append(':')
                .Append(callerLineNumber)
                .Append(']')
                .Append(' ');
        }
        public void AppendLiteral(string s) => _sb.Append(s);
        public void AppendFormatted<T>(T value)
        {
            if (value == null)
            {
                _sb.Append("null");
                return;
            }
            if (value is IFormattable formattable)
            {
                if (value is ISpanFormattable spanFormattable)
                {
                    Span<char> buffer = stackalloc char[64];
                    if (spanFormattable.TryFormat(buffer, out int charsWritten, format: default, null))
                    {
                        _sb.Append(buffer.Slice(0, charsWritten));
                    }
                    else
                    {
                        _sb.Append(formattable.ToString(null, null));
                    }
                }
                else
                {
                    _sb.Append(formattable.ToString(null, null));
                }
            }
            else
            {
                _sb.Append(value.ToString());
            }
        }
        public void AppendFormatted<T>(T value, string? format)
        {
            if (value == null)
            {
                _sb.Append("null");
                return;
            }
            if (value is IFormattable formattable)
            {
                if (value is ISpanFormattable spanFormattable)
                {
                    Span<char> buffer = stackalloc char[64];
                    if (spanFormattable.TryFormat(buffer, out int charsWritten, format.AsSpan(), null))
                    {
                        _sb.Append(buffer.Slice(0, charsWritten));
                    }
                    else
                    {
                        _sb.Append(formattable.ToString(format, null));
                    }
                }
                else
                {
                    _sb.Append(formattable.ToString(format, null));
                }
            }
            else
            {
                _sb.Append(value.ToString());
            }
        }

        public void AppendFormatted(ReadOnlySpan<char> value) => _sb.Append(value);
        public void AppendFormatted(string? value) => _sb.Append(value);
        public void AppendFormatted(int value) => _sb.Append(value);
        public void AppendFormatted(char value) => _sb.Append(value);
        public void AppendFormatted(float value) => _sb.Append(value);
        public void AppendFormatted(bool value) => _sb.Append(value);
        public override string ToString() => _sb.ToString();
    }

    public static class ULog
    {
        [ThreadStatic]
        private static StringBuilder _sb = new(256);

        [HideInCallstack]
        [Conditional("ULOG_LEVEL_TRACE")]
        public static void Trace(InterpolatedStringHandler handler)
        {
            UnityEngine.Debug.Log(handler.ToString());
        }
        [HideInCallstack]
        [Conditional("ULOG_LEVEL_TRACE")]
        public static void Trace(
            string message,
            UnityEngine.Object context = null,
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            UnityEngine.Debug.Log(FormatMessage(message, callerFilePath, callerMemberName, callerLineNumber), context);
        }

        [HideInCallstack]
        [Conditional("ULOG_LEVEL_TRACE")]
        [Conditional("ULOG_LEVEL_DEBUG")]
        public static void Debug(InterpolatedStringHandler handler)
        {
            UnityEngine.Debug.Log(handler.ToString());
        }
        [HideInCallstack]
        [Conditional("ULOG_LEVEL_TRACE")]
        [Conditional("ULOG_LEVEL_DEBUG")]
        public static void Debug(
            string message,
            UnityEngine.Object context = null,
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            UnityEngine.Debug.Log(FormatMessage(message, callerFilePath, callerMemberName, callerLineNumber), context);
        }

        [HideInCallstack]
        //[Conditional("ULOG_LEVEL_TRACE")]
        //[Conditional("ULOG_LEVEL_DEBUG")]
        //[Conditional("ULOG_LEVEL_INFO")]
        public static void Info(InterpolatedStringHandler handler)
        {
            UnityEngine.Debug.Log(handler.ToString());
        }
        [HideInCallstack]
        //[Conditional("ULOG_LEVEL_TRACE")]
        //[Conditional("ULOG_LEVEL_DEBUG")]
        //[Conditional("ULOG_LEVEL_INFO")]
        public static void Info(
            string message,
            UnityEngine.Object context = null,
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            UnityEngine.Debug.Log(FormatMessage(message, callerFilePath, callerMemberName, callerLineNumber), context);
        }

        [HideInCallstack]
        [Conditional("ULOG_LEVEL_TRACE")]
        [Conditional("ULOG_LEVEL_DEBUG")]
        [Conditional("ULOG_LEVEL_INFO")]
        [Conditional("ULOG_LEVEL_WARN")]
        public static void Warn(InterpolatedStringHandler handler)
        {
            UnityEngine.Debug.LogWarning(handler.ToString());
        }
        [HideInCallstack]
        [Conditional("ULOG_LEVEL_TRACE")]
        [Conditional("ULOG_LEVEL_DEBUG")]
        [Conditional("ULOG_LEVEL_INFO")]
        [Conditional("ULOG_LEVEL_WARN")]
        public static void Warn(
            string message,
            UnityEngine.Object context = null,
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            UnityEngine.Debug.LogWarning(FormatMessage(message, callerFilePath, callerMemberName, callerLineNumber), context);
        }

        [HideInCallstack]
        [Conditional("ULOG_LEVEL_TRACE")]
        [Conditional("ULOG_LEVEL_DEBUG")]
        [Conditional("ULOG_LEVEL_INFO")]
        [Conditional("ULOG_LEVEL_WARN")]
        [Conditional("ULOG_LEVEL_ERROR")]
        public static void Error(InterpolatedStringHandler handler)
        {
            UnityEngine.Debug.LogError(handler.ToString());
        }
        [HideInCallstack]
        [Conditional("ULOG_LEVEL_TRACE")]
        [Conditional("ULOG_LEVEL_DEBUG")]
        [Conditional("ULOG_LEVEL_INFO")]
        [Conditional("ULOG_LEVEL_WARN")]
        [Conditional("ULOG_LEVEL_ERROR")]
        public static void Error(
            string message,
            UnityEngine.Object context = null,
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            UnityEngine.Debug.LogError(FormatMessage(message, callerFilePath, callerMemberName, callerLineNumber), context);
        }

        private static string FormatMessage(string message, string callerFilePath, string callerMemberName, int callerLineNumber)
        {
            _sb.Clear();
            ReadOnlySpan<char> fileName = System.IO.Path.GetFileName(callerFilePath.AsSpan());
            return _sb
                .Append('[')
                .Append(fileName)
                .Append(':')
                .Append(callerMemberName)
                .Append(':')
                .Append(callerLineNumber)
                .Append(']')
                .Append(' ')
                .Append(message)
                .ToString();
        }
    }
}
