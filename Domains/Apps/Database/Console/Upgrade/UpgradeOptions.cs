namespace Allors.Console
{
    using CommandLine;

    [Verb("upgrade", HelpText = "Upgrade the database.")]
    public class UpgradeOptions
    {
        [Option('f', "file", Required = false, HelpText = "File to load from.")]
        public string File { get; set; }
    }
}
