namespace Allors.Commands
{
    using System;
    using System.Xml;
    using System.IO;

    public class Save : Command
    {
        public override void Execute()
        {
            var database = this.SnapshotDatabase;

            using (var stream = File.Create(this.PopulationFileName))
            {
                using (var writer = XmlWriter.Create(stream))
                {
                    Console.WriteLine("Saving to " + this.PopulationFileName);

                    database.Save(writer);

                    Console.WriteLine("Saved to " + this.PopulationFileName);
                }
            }
        }
    }
}
