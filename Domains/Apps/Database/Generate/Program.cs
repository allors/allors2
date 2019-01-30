namespace Allors
{
    using System;
    using System.IO;

    using Allors.Development.Repository.Tasks;

    class Program
    {
        static int Main(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    return Default();
                case 2:
                    return Generate.Execute(args[0], args[1]).ErrorOccured ? 1 : 0;
                default:
                    return 1;
            }
        }

        private static int Default()
        {
            string[,] config =
                {
                    { "../Base/Database/Templates/domain.cs.stg", "DataBase/Domain/Generated" },
                    { "../Base/Database/Templates/uml.cs.stg", "DataBase/Diagrams/Generated" },

                    { "../Base/Workspace/Typescript/modules/Templates/meta.ts.stg", "Workspace/Typescript/Domain/src/allors/meta/generated" },
                    { "../Base/Workspace/Typescript/modules/Templates/domain.ts.stg", "Workspace/Typescript/Domain/src/allors/domain/generated" },

                    { "../Base/Workspace/Typescript/modules/Templates/meta.ts.stg", "Workspace/Typescript/Intranet/src/allors/meta/generated" },
                    { "../Base/Workspace/Typescript/modules/Templates/domain.ts.stg", "Workspace/Typescript/Intranet/src/allors/domain/generated" },
                };

            for (var i = 0; i < config.GetLength(0); i++)
            {
                var template = config[i, 0];
                var output = config[i, 1];

                Console.WriteLine("-> " + output);

                RemoveDirectory(output);

                var log = Generate.Execute(template, output);
                if (log.ErrorOccured)
                {
                    return 1;
                }
            }


            return 0;
        }

        private static void RemoveDirectory(string output)
        {
            var directoryInfo = new DirectoryInfo(output);
            if (directoryInfo.Exists)
            {
                try
                {
                    directoryInfo.Delete(true);
                }
                catch
                {
                }
            }
        }
    }
}
