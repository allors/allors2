namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("c4b51143-7e9c-4f1d-a34f-cc99f29a12e9")]
    #endregion
    public partial class Tolerance : PartSpecification 
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

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Approve(){}




        #endregion

    }
}