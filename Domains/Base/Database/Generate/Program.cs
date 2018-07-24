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
                    { "Database/Templates/domain.cs.stg", "Database/Domain/generated" },
                    { "Database/Templates/uml.cs.stg", "Database/Diagrams" },

                    { "Workspace/Csharp/Templates/uml.cs.stg", "Workspace/CSharp/Diagrams" },
                    { "Workspace/Csharp/Templates/meta.cs.stg", "Workspace/CSharp/Meta/generated" },
                    { "Workspace/Csharp/Templates/domain.cs.stg", "Workspace/CSharp/Domain/generated" },

                    { "Workspace/Typescript/modules/Templates/meta.ts.stg", "Workspace/Typescript/modules/Domain/src/allors/meta/generated" },
                    { "Workspace/Typescript/modules/Templates/domain.ts.stg", "Workspace/Typescript/modules/Domain/src/allors/domain/generated" },

                    { "Workspace/Typescript/modules/Templates/meta.ts.stg", "Workspace/Typescript/modules/Angular/src/allors/meta/generated" },
                    { "Workspace/Typescript/modules/Templates/domain.ts.stg", "Workspace/Typescript/modules/Angular/src/allors/domain/generated" },

                    { "Workspace/Typescript/modules/Templates/meta.ts.stg", "Workspace/Typescript/modules/Material/src/allors/meta/generated" },
                    { "Workspace/Typescript/modules/Templates/domain.ts.stg", "Workspace/Typescript/modules/Material/src/allors/domain/generated" },

                    { "Workspace/Typescript/modules/Templates/meta.ts.stg", "Workspace/Typescript/modules/Promise/src/allors/meta/generated" },
                    { "Workspace/Typescript/modules/Templates/domain.ts.stg", "Workspace/Typescript/modules/Promise/src/allors/domain/generated" },
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
