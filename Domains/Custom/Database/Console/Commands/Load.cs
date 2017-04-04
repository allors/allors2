namespace Allors.Commands
{
    using System;
    using System.Xml;

    public class Load : Command
    {
        public override void Execute()
        {
            var database = this.RepeatableReadDatabase; // Or Serializable

            using (var reader = XmlReader.Create(this.PopulationFileName))
            {
                Console.WriteLine("Loading from " + this.PopulationFileName);

                database.Load(reader);

                Console.WriteLine("Loaded from " + this.PopulationFileName);
            }
        }
    }
}
