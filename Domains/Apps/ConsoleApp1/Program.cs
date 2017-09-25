using System;

namespace ConsoleApp1
{
    using Allors.Meta;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var val = MetaPopulation.Instance.Validate();

        }
    }
}
