using System;
using System.Collections.Generic;
using System.Text;

namespace GameCtor.DevToolbox
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder Append(this StringBuilder sb, string value, int count)
        {
            for (int i = 0; i < count; i++)
            {
                sb.Append(value);
            }
            return sb;
        }
    }

    public sealed class SourceWriter : IDisposable
    {
        private readonly StringBuilder sb = new StringBuilder();
        private readonly string indentString = "    ";
        private int indentLevel;
        private bool excludeClosingBrace;

        public SourceWriter Append(string text)
        {
            sb.Append(text);
            return this;
        }

        public SourceWriter Indent()
        {
            indentLevel++;
            return this;
        }

        public SourceWriter Unindent()
        {
            indentLevel--;
            return this;
        }

        public SourceWriter AppendLine()
        {
            sb.AppendLine();
            return this;
        }

        public SourceWriter AppendLine(string text)
        {
            sb.Append(indentString, indentLevel);
            sb.AppendLine(text);
            return this;
        }

        public SourceWriter AppendLineFormat(string text, params string[] args)
        {
            sb.Append(indentString, indentLevel);
            sb.AppendFormat(text, args);
            sb.AppendLine();
            return this;
        }

        public SourceWriter AppendLines(IEnumerable<string> texts)
        {
            indentLevel++;
            foreach (var text in texts)
            {
                sb.Append(indentString, indentLevel);
                sb.Append(text);
                sb.Append(",\n");
            }
            sb.Remove(sb.Length - 2, 2);
            indentLevel--;
            return this;
        }

        public SourceWriter Block(string text = "")
        {
            AppendLine(text);
            AppendLine("{");
            indentLevel++;
            return this;
        }

        public SourceWriter BlockFormat(string format, params string[] args)
        {
            AppendLineFormat(format, args);
            AppendLine("{");
            indentLevel++;
            return this;
        }

        public SourceWriter BlockFormatWithoutBraces(string format, params string[] args)
        {
            AppendLineFormat(format, args);
            indentLevel++;
            excludeClosingBrace = true;
            return this;
        }

        // Make this an extension.
        public SourceWriter Namespace(string namespaceName)
        {
            if (namespaceName == "null") return this;
            return BlockFormat("namespace {0}", namespaceName);
        }

        public void Dispose()
        {
            if (indentLevel > 0)
            {
                indentLevel--;
                if (excludeClosingBrace)
                {
                    excludeClosingBrace = false;
                }
                else
                {
                    AppendLine("}");
                }
            }
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }
}
