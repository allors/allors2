namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("D02C3738-1095-4B86-A1FB-C56A841A8082")]
    #endregion
    public partial class RequestItemVersion : IRequestItem
    {
        #region inherited properties
        public string InternalComment { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public Requirement[] Requirements { get; set; }
        public Deliverable Deliverable { get; set; }
        public ProductFeature ProductFeature { get; set; }
        public NeededSkill NeededSkill { get; set; }
        public Product Product { get; set; }
        public decimal MaximumAllowedPrice { get; set; }
        public DateTime RequiredByDate { get; set; }
        public RequestItemObjectState CurrentObjectState { get; set; }
       #endregion

        #region Allors
        [Id("90792C00-8F72-4236-998A-F13A37662A8B")]
        [AssociationId("5AC75220-4C8B-41B0-A5BB-4A40B5117D0F")]
        [RoleId("AC8342C8-666A-4595-8EFE-78A1BB8FFC85")]
        #endregion
        [Workspace]
        public DateTime TimeStamp { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}