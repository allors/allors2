namespace Allors.Console
{
    using CommandLine;

    [Verb("load", HelpText = "Load the database.")]
    public class LoadOptions
    {
        [Option('f', "file", Required = false, HelpText = "File to load from.")]
        public string File { get; set; }
    }
}
