namespace Allors.Scheduler
{
    public class LogAdapter : ILogger
    {
        private readonly NLog.Logger subject;

        public LogAdapter(NLog.Logger subject)
        {
            this.subject = subject;
        }

        public void Debug(string message)
        {
            this.subject.Debug(message);
        }

        public void Info(string message)
        {
            this.subject.Info(message);
        }

        public void Warn(string message)
        {
            this.subject.Warn(message);
        }

        public void Error(string message)
        {
            this.subject.Error(message);
        }
    }
}
