using System;
using NLog;

namespace OpenLawOffice.WinClient.ErrorHandling
{
    public abstract class ErrorBase
    {
        private int? _id;
        private DateTime _created;
        private DateTime? _thrown;

        public bool IsRegistered { get { return _id.HasValue; } }
        public LevelType Level { get; set; }
        public int? Id { get { return _id; } }
        public string SimpleMessage { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public string Source { get; set; }

        public ErrorBase()
        {
            _id = null;
        }

        public virtual void Init(int id)
        {
            _id = id;
            _created = DateTime.Now;
        }

        public virtual void Throw(object data)
        {
            if (!IsRegistered)
                throw new ErrorNotRegisteredException();

            _thrown = DateTime.Now;
            Log();
        }

        public virtual void Log()
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            LogEventInfo eventInfo = new LogEventInfo()
            {
                LoggerName = logger.Name,
                TimeStamp = _created,
                Exception = Exception,
                Message = Message
            };

            eventInfo.Properties["Id"] = Id;
            eventInfo.Properties["Source"] = Source;
            eventInfo.Properties["SimpleMessage"] = SimpleMessage;

            switch (Level)
            {
                case LevelType.Fatal:
                    eventInfo.Level = LogLevel.Fatal;
                    break;
                case LevelType.Error:
                    eventInfo.Level = LogLevel.Error;
                    break;
                case LevelType.Warn:
                    eventInfo.Level = LogLevel.Warn;
                    break;
                case LevelType.Info:
                    eventInfo.Level = LogLevel.Info;
                    break;
                default:
                    throw new ErrorLevelNotSetException();
            }

            logger.Log(eventInfo);
        }
    }
}
