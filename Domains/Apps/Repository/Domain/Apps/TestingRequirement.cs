namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("a06befc5-c347-4ffb-9391-2a099fca5145")]
    #endregion
    public partial class TestingRequirement : PartSpecification 
    {
        #region inherited properties
        public PartSpecificationStatus[] PartSpecificationStatuses { get; set; }

        public PartSpecificationObjectState CurrentObjectState { get; set; }

        public DateTime DocumentationDate { get; set; }

        public PartSpecificationStatus CurrentPartSpecificationStatus { get; set; }

        public string Description { get; set; }

        public Guid UniqueId { get; set; }

        public string Comment { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Approve(){}




        #endregion

    }
}