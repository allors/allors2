namespace Allors.Repository.Domain
{
    #region Allors
    [Id("94be4938-77c1-488f-b116-6d4daeffcc8d")]
    #endregion
    public partial class Order : Transitional 
    {
        #region inherited properties
        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("26560f5b-9552-42ea-861f-8a653abeb16e")]
        [AssociationId("d0cdd4a7-6323-4571-85e0-875a5adc56f7")]
        [RoleId("f97ce5e4-88e2-4a4f-a26c-01a68db33efa")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public OrderObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("5aa7fa5c-c0a5-4384-9b24-9ecef17c4848")]
        [AssociationId("ffcb8a00-571f-4032-b038-82b438f96f74")]
        [RoleId("cf1629aa-2aa0-4dc3-9873-fbf3008352ac")]
        #endregion
        public int Amount { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion
    }
}