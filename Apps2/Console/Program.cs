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
            Development.Repository.Tasks.Generate.Execute("../../../../Base/Templates/Base/domain.cs.stg", "../../../Domain/Generated/Base/Domain");
            Console.WriteLine("Completed");
            Console.ReadKey();
        }
    }
}
