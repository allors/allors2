using Allors.Meta;

namespace Allors
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            Generate();
        }

        public static void Generate()
        {
            //Development.Repository.Tasks.Generate.Execute("../../../Templates/workspace.meta.ts.stg", "../../../Website/Allors/Client/Generated/meta");
            //Development.Repository.Tasks.Generate.Execute("../../../Templates/workspace.domain.ts.stg", "../../../Website/Allors/Client/Generated/domain");

            Development.Repository.Tasks.Generate.Execute("../../../Templates/workspace.meta.cs.stg", "../../../Workspace.Meta/Generated");
            //Development.Repository.Tasks.Generate.Execute("../../../Templates/workspace.domain.cs.stg", "../../../Workspace.Domain/Generated");

            //Development.Repository.Tasks.Generate.Execute("../../../Templates/workspace.uml.cs.stg", "../../../Desktop.Diagrams");

            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
