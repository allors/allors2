// <copyright file="Program.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Tools.Cmd
{
    using System;
    using System.IO;
    using NLog;
    using Repository;
    using Repository.Roslyn;

    public class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int Main(string[] args)
        {
            try
            {
                if (args.Length < 3)
                {
                    Logger.Error("missing required arguments");
                }

                RepositoryGenerate(args);
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

        private static void RepositoryGenerate(string[] args)
        {
            var projectPath = args[0];
            var template = args[1];
            var output = args[2];

            var fileInfo = new FileInfo(projectPath);

            Logger.Info("Generate " + fileInfo.FullName);
            Generate.Execute(fileInfo.FullName, template, output);
        }
    }
}
