namespace Allors.Meta
{
    public partial class MetaCommunicationEvent
    {
        internal override void AppsExtend()
        {
            this.Cancel.Workspace = true;
            this.Close.Workspace = true;
            this.Reopen.Workspace = true;

            this.ActualStart.RelationType.Workspace = true;
            this.ActualEnd.RelationType.Workspace = true;
            this.FromParties.RelationType.Workspace = true;
            this.ToParties.RelationType.Workspace = true;
            this.Subject.RelationType.Workspace = true;
            this.CurrentCommunicationEventStatus.RelationType.Workspace = true;
            this.Description.RelationType.Workspace = true;
            this.EventPurposes.RelationType.Workspace = true;
            this.FromContactMechanisms.RelationType.Workspace = true;
            this.InvolvedParties.RelationType.Workspace = true;
            this.Note.RelationType.Workspace = true;
            this.Owner.RelationType.Workspace = true;
            this.ToContactMechanisms.RelationType.Workspace = true;
            this.ScheduledStart.RelationType.Workspace = true;
            this.ScheduledEnd.RelationType.Workspace = true;
        }
    }
}