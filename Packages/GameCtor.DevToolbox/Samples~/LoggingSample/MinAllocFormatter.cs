using System;
using System.IO;
using static GameCtor.ULogging.ULogFormatHelpers;

namespace GameCtor.ULogging
{
    /// <summary>
    /// A formatter with the following format:
    /// {timestamp} {logLevel} [{fileName}::{memberName}::{lineNumber}]-> {message}{payload}
    /// </summary>
    /// <remarks>Just messing around with <see cref="Span{T}"/></remarks>
    public sealed class MinAllocFormatter : BaseLogFormatter
    {
        public MinAllocFormatter(string timestampFormat = "HH:mm:ss") : base(timestampFormat)
        {
        }

        public override string GetFormattedString(
            ULogLevel logLevel,
            string message,
            object payload,
            string memberName,
            string filePath,
            int lineNumber)
        {
            LevelToLabelMap.TryGetValue(logLevel, out var levelLabel);
            var start = filePath.LastIndexOf(Path.DirectorySeparatorChar) + 1;
            var fileNameLength = filePath.Length - start;
            string payloadString = GetPayloadString(payload, "\npayload: ");

            Span<char> timestampBuffer = stackalloc char[_timestampFormat.Length];
            GetTimestamp().TryFormat(timestampBuffer, out int timestampCharCount, _timestampFormat.AsSpan());

            Span<char> lineNumberBuffer = stackalloc char[5];
            lineNumber.TryFormat(lineNumberBuffer, out var lineNumberCharCount);

            var length =
                11 +
                timestampCharCount +
                levelLabel.Length +
                fileNameLength +
                memberName.Length +
                lineNumberCharCount +
                message.Length +
                payloadString.Length;

            var state = new State
            {
                LevelLabel = levelLabel,
                FilePath = filePath,
                MemberName = memberName,
                Message = message,
                PayloadString = payloadString,
                LineNumber = lineNumber,
                Timestamp = GetTimestamp(),
                TimestampFormat = _timestampFormat,
                FileNameStartIndex = start,
            };

            var str = string.Create(length, state, static (buffer, state) =>
            {
                Span<char> timestampBuffer = stackalloc char[state.TimestampFormat.Length];
                state.Timestamp.TryFormat(timestampBuffer, out int timestampCharCount, state.TimestampFormat.AsSpan());
                ReadOnlySpan<char> timestamp = timestampBuffer.Slice(0, timestampCharCount);
                timestamp.CopyTo(buffer);
                buffer = buffer.Slice(timestamp.Length);

                Write(" ".AsSpan(), ref buffer);
                Write(state.LevelLabel.AsSpan(), ref buffer);
                Write(" [".AsSpan(), ref buffer);
                Write(state.FilePath.AsSpan(state.FileNameStartIndex), ref buffer);
                Write("::".AsSpan(), ref buffer);
                Write(state.MemberName.AsSpan(), ref buffer);
                Write("::".AsSpan(), ref buffer);

                Span<char> lineNumberBuffer = stackalloc char[5];
                state.LineNumber.TryFormat(lineNumberBuffer, out var lineNumberCharCount);
                lineNumberBuffer.Slice(0, lineNumberCharCount).CopyTo(buffer);
                buffer = buffer.Slice(lineNumberCharCount);

                Write("]-> ".AsSpan(), ref buffer);
                Write(state.Message.AsSpan(), ref buffer);
                state.PayloadString.AsSpan().CopyTo(buffer);
            });

            return str;
        }

        static void Write(ReadOnlySpan<char> content, ref Span<char> buffer) 
        {
            content.CopyTo(buffer); 
            buffer = buffer.Slice(content.Length); 
        }

        internal struct State
        {
            public string LevelLabel;
            public string FilePath;
            public string MemberName;
            public string Message;
            public string PayloadString;
            public DateTime Timestamp;
            public int LineNumber;
            public int FileNameStartIndex;
            public string TimestampFormat;
        }
    }
}
