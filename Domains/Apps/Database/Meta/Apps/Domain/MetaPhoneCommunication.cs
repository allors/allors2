namespace Allors.Meta
{
    public partial class MetaPhoneCommunication
    {
        internal override void AppsExtend()
        {
            this.Delete.Workspace = true;

            this.Callers.RelationType.Workspace = true;
            this.Receivers.RelationType.Workspace = true;
            this.IncomingCall.RelationType.Workspace = true;
            this.LeftVoiceMail.RelationType.Workspace = true;
        }
    }
}