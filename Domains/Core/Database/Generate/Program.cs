namespace Allors
{
    using Allors.Development.Repository.Tasks;

    class Program
    {
        static int Main(string[] args)
        {
            var log = args.Length > 1 ? 
                Generate.Execute(args[0], args[1]) : 
                Generate.Execute("../../Templates/docs.html.stg", "../../Docs");

            return log.ErrorOccured ? 1 : 0;
        }
    }
}
