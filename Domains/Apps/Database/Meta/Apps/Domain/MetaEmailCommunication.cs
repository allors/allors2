namespace Allors.Meta
{
    public partial class MetaEmailCommunication
    {
        public Tree AngularHome;

        internal override void AppsExtend()
        {
            this.Delete.Workspace = true;

            this.Addressees.RelationType.Workspace = true;
            this.BlindCopies.RelationType.Workspace = true;
            this.CarbonCopies.RelationType.Workspace = true;
            this.EmailTemplate.RelationType.Workspace = true;
            this.Originator.RelationType.Workspace = true;
        }
    }
}