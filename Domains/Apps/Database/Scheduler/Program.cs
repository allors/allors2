namespace Allors.Scheduler
{
    using System;
    using System.Data.SqlClient;

    using NLog;

    public class Program
    {
        private const string UsageMessage = "Invalid Syntax.\nUsage: scheduler {a | h | d | w | m}";

        private const int Success = 0;
        private const int Failure = 1;

        public static int Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                var argument = (args.Length == 0 || string.IsNullOrWhiteSpace(args[0])) ? 'h' : args[0].ToLower()[0];

                Scheduler scheduler;
                switch (argument)
                {
                    case 'i':
                        scheduler = new ImmediateScheduler();
                        break;

                    case 'h':
                        scheduler = new HourlyScheduler();
                        break;

                    case 'd':
                        scheduler = new DailyScheduler();
                        break;

                    case 'w':
                        scheduler = new WeeklyScheduler();
                        break;

                    case 'm':
                        scheduler = new MonthlyScheduler();
                        break;

                    default:
                        throw new ArgumentException();
                }

                scheduler.Schedule();
            }
            catch (SqlException sqlException)
            {
                if (sqlException.ToString().ToLowerInvariant().Contains("snapshot"))
                {
                    logger.Warn("Snapshot isolation conflict.");
                    return Success;
                }

                logger.Error(sqlException);
                return Failure;
            }
            catch (ArgumentException ae)
            {
                logger.Error(ae.Message + "-" + UsageMessage);
                return Failure;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return Failure;
            }

            return Success;
        }
    }
}
