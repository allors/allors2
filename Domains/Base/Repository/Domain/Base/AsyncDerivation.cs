namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("7ca39473-bba1-49af-a343-f3cc7abe14d5")]
    #endregion
    public partial class AsyncDerivation : Deletable 
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("040ae9ff-c5ba-490a-88b0-0e044c116039")]
        [AssociationId("a48f2f25-7a77-42b2-b91d-6f169189897b")]
        [RoleId("4e02fba4-eae4-4c7d-b174-e2326b81fdc4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public AsyncDerivable AsyncDerivable { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Delete(){}
        #endregion
    }
}