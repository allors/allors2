namespace Allors.Tools.Cmd
{
    using System;
    using System.IO;

    using Allors.Tools.Repository;

    using NLog;

    using Repository.Tasks;

    public class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    Logger.Error("missing required arguments");
                }

                var context = args[0];

                switch (context.ToLowerInvariant())
                {
                    case "repository":
                        Repository(args);
                        break;

                    default:
                        Logger.Error("unknown context");
                        break;
                }
            }
            catch (RepositoryException e)
            {
                Logger.Error(e.Message);
                return 1;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                Logger.Info("Finished with errors");
                return 1;
            }

            Logger.Info("Finished");
            return 0;
        }

        private static void Repository(string[] args)
        {
            var command = args[1];

            switch (command.ToLowerInvariant())
            {
                case "generate":
                    RepositoryGenerate(args);
                    break;

                default:
                    Logger.Error("unknown command");
                    break;
            }
        }

        private static void RepositoryGenerate(string[] args)
        {
            var solutionPath = args[2];
            var projectName = args[3];
            var template = args[4];
            var output = args[5];

            var fileInfo = new FileInfo(solutionPath);

            Logger.Info("Generate " + fileInfo.FullName);
            Generate.Execute(fileInfo.FullName, projectName, template, output);
        }
    }
}
