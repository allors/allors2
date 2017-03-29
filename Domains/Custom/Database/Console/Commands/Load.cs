namespace Allors.Commands
{
    using System.Xml;

    public class Load : Command
    {
        public override void Execute()
        {
            var database = this.RepeatableReadDatabase; // Or Serializable

            using (var reader = new XmlTextReader(this.PopulationFileName))
            {
                this.Logger.Info("Loading from " + this.PopulationFileName);

                database.Load(reader);

                this.Logger.Info("Loaded from " + this.PopulationFileName);
            }
        }
    }
}
