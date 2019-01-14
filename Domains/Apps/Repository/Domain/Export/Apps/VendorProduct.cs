namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("C870A795-E881-40CD-A4D7-AB64B9C063E7")]
    #endregion
    public partial class VendorProduct : PartyRelationship
    {
        #region inherited properties

        public Party[] Parties { get; set; }
        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("835E728A-48F5-4FDF-AFD0-FA3CC3002E2D")]
        [AssociationId("EBB702D5-287C-42C1-9386-C24C242CA77F")]
        [RoleId("EC643629-830A-4BAF-9DC2-02F899C63FD0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public Product Product{ get; set; }

        #region Allors
        [Id("DFB7A0D6-B7D6-4702-BFEC-6EDCDBB6FF08")]
        [AssociationId("8E96955C-6BD8-4453-992E-0ABFAD54E3BF")]
        [RoleId("3585AF45-B8B3-4481-BF5B-C1545C434D40")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InternalOrganisation InternalOrganisation { get; set; }

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