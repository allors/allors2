namespace src.allors.material.apps.objects.communicationevent.list
{
    using Allors.Domain;

    public partial class CommunicationEventListComponent
    {
        public void Select(CommunicationEvent communicationEvent)
        {
            var row = this.Table.FindRow(communicationEvent);
            var cell = row.FindCell("subject");
            cell.Click();
        }
    }
}
