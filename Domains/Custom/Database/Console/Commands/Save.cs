namespace Allors.Commands
{
    using System.Text;
    using System.Xml;

    public class Save : Command
    {
        public override void Execute()
        {
            var database = this.SnapshotDatabase;

            using (var writer = new XmlTextWriter(this.PopulationFileName, Encoding.UTF8))
            {
                this.Logger.Info("Saving to " + this.PopulationFileName);

                database.Save(writer);

                this.Logger.Info("Saved to " + this.PopulationFileName);
            }
        }
    }
}
