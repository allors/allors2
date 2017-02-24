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
            var config = new System.Collections.Generic.Dictionary<string, string>()
                             {
                                { "../Base/Database/Templates/domain.cs.stg", "Domain/Generated" },
                                { "../Base/Database/Templates/uml.cs.stg", "Domain/Domain.Diagrams" },
                             };

            foreach (var entry in config)
            {
                var log = Generate.Execute(entry.Key, entry.Value);
                if (log.ErrorOccured)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
