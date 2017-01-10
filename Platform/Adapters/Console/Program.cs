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
            var log = Development.Repository.Tasks.Generate.Execute("../../../Templates/adapters.cs.stg", "../../../Domain/Generated/Adapters");
            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
