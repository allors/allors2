using System;

namespace Allors
{
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
                                { "../Base/Database/Templates/uml.cs.stg", "DataBase/Diagrams" },
                                { "../Base/Workspace/CSharp/Templates/meta.cs.stg", "Workspace/CSharp/Meta/Generated" },
                                { "../Base/Workspace/CSharp/Templates/domain.cs.stg", "Workspace/CSharp/Domain/Generated" },
                                { "../Base/Workspace/CSharp/Templates/uml.cs.stg", "Workspace/CSharp/Diagrams" },
                                { "../Base/Workspace/Typescript/Templates/meta.ts.stg", "Workspace/Typescript/Meta/src/meta/generated" },
                                { "../Base/Workspace/Typescript/Templates/meta.ts.stg", "Workspace/Typescript/Domain/src/meta/generated" },
                                { "../Base/Workspace/Typescript/Templates/domain.ts.stg", "Workspace/Typescript/Domain/src/domain/generated" },
                                { "../Base/Workspace/Typescript/Templates/meta.ts.stg", "Workspace/Typescript/Angular/src/meta/generated" },
                                { "../Base/Workspace/Typescript/Templates/domain.ts.stg", "Workspace/Typescript/Angular/src/domain/generated" },
                             };

            for (var i = 0; i < config.GetLength(0); i++)
            {
                var template = config[i, 0];
                var output = config[i, 1];

                Console.WriteLine("-> " + output);
                var log = Generate.Execute(template, output);
                if (log.ErrorOccured)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
