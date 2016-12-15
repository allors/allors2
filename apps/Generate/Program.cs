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
            try
            {
                Development.Repository.Tasks.Generate.Execute(
                    "../../../../platform/base/Templates/domain.cs.stg",
                    "../../../Domain/Generated");
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "\n" + e.StackTrace);    
            }

            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
