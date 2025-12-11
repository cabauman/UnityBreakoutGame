using UnityEngine;
using UnityEngine.UI;

namespace GameCtor.ULogging.Samples
{
    public class LoggingSample : MonoBehaviour
    {
        [SerializeField] private HandlebarsLogFormatter _handlebarsFormatter;
        [SerializeField] private InputField _messageInput;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Init()
        {
            // ULog initialization can be done here.

            // As a personal preference, I like to disable stack traces for all log types except Error.
            // Especially since caller information is already included in the log message.
            //Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            //Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
        }

        private void Awake()
        {
            ULog.Level = ULogLevel.Info;
            ChangeFormatter(0);
        }

        void Start()
        {
            ULog.Debug("Hello World!", this, new { Name = "Colt", Num = Vector3.right });
        }

        public void ChangeLevel(int index)
        {
            ULog.Level = (ULogLevel)index;
        }

        public void ChangeFormatter(int index)
        {
            ILogFormatter formatter;
            switch (index)
            {
                case 0:
                    formatter = new EditorLogFormatter();
                    break;
                case 1:
                    formatter = new SourceLogFormatter();
                    break;
                case 2:
                    formatter = new JsonLogFormatter();
                    break;
                case 3:
                    formatter = _handlebarsFormatter;
                    break;
                default:
                    formatter = new PlainLogFormatter();
                    break;
            }

            var logger = new UnityLogger(formatter);
            var config = new ULogConfig
            {
                Level = ULog.Level,
                Loggers = new BaseLogger[] { logger },
            };
            ULog.Init(config);
        }

        public void LogError()
        {
            try
            {
                ErrorMethod();
            }
            catch (System.Exception ex)
            {
                ULog.Error(_messageInput.text, payload: ex);
            }
        }

        public void LogWarn()
        {
            ULog.Warn(_messageInput.text);
        }

        public void LogInfo()
        {
            ULog.Info(_messageInput.text);
        }

        public void LogDebug()
        {
            ULog.Debug(_messageInput.text);
        }

        public void LogTrace()
        {
            ULog.Trace(_messageInput.text);
        }

        public void ErrorMethod()
        {
            throw new System.InvalidOperationException("Test Exception");
        }
    }
}
