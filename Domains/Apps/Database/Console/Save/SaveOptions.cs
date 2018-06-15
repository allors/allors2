namespace Allors.Console
{
    using CommandLine;

    [Verb("save", HelpText = "Save the database.")]
    public class SaveOptions
    {
        [Option('f', "file", Required = false, HelpText = "File to save to.")]
        public string File { get; set; }
    }
}
