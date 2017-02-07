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
            var log = Development.Repository.Tasks.Generate.Execute("../../../Templates/domain.cs.stg", "../../../Domain/Generated/core");
            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
