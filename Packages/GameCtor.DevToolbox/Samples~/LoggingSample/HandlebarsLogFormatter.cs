using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static GameCtor.ULogging.ULogFormatHelpers;

namespace GameCtor.ULogging
{
    /// <summary>
    /// A custom formatter that uses Handlebars.Net templating.
    /// https://github.com/Handlebars-Net/Handlebars.Net
    /// </summary>
    public sealed class HandlebarsLogFormatter : MonoBehaviour, ILogFormatter
    {
        [SerializeField]
        private string _timestampFormat = "HH:mm:ss";

        [SerializeField]
        [TextArea(3, 10)]
        [Tooltip("timestamp, logLevel, fileName, memberName, lineNumber, message")]
        private string _template = "{{logLevel}} [{{fileName}}::{{memberName}}::{{lineNumber}}]-> {{message}}";

        private HandlebarsTemplate<object, object> _handlebarsTemplate;
        private readonly Dictionary<string, string> _dataMap = new();

        public string GetFormattedString(
            ULogLevel logLevel,
            string message,
            object payload,
            string memberName,
            string filePath,
            int lineNumber)
        {
            _handlebarsTemplate ??= Handlebars.Compile(_template);
            LevelToLabelMap.TryGetValue(logLevel, out var levelLabel);
            var start = filePath.LastIndexOf(Path.DirectorySeparatorChar) + 1;
            var fileName = filePath.Substring(start, filePath.Length - start);
            string payloadString = GetPayloadString(payload, "\npayload: ");

            _dataMap["logLevel"] = levelLabel;
            _dataMap["fileName"] = fileName;
            _dataMap["memberName"] = memberName;
            _dataMap["lineNumber"] = lineNumber.ToString();
            _dataMap["message"] = message;
            _dataMap["timestamp"] = DateTime.Now.ToString(_timestampFormat);

            return _handlebarsTemplate(_dataMap) + payloadString;
        }
    }
}
