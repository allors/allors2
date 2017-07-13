namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("37b665a5-9f73-4002-b7d2-7ed6987fe09a")]
    #endregion
    public partial class PerformanceSpecification : PartSpecification 
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