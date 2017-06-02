namespace Allors.Meta
{
    public partial class MetaFaceToFaceCommunication
    {
        public Tree AngularHome;

        internal override void AppsExtend()
        {
            this.Delete.Workspace = true;

            this.Location.RelationType.Workspace = true;
            this.Participants.RelationType.Workspace = true;
        }
    }
}