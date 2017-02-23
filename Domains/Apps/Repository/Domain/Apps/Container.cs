namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("9ec6dae1-439e-4b19-b4dc-885e1ed943d7")]
    #endregion
	public partial interface Container : AccessControlledObject 
    {


        #region Allors
        [Id("a8279f40-4624-4aa9-9e61-fc01f880ca17")]
        [AssociationId("15f3df8c-c20e-4162-b5f8-1e031001f11f")]
        [RoleId("33da1e6a-60bd-4c50-9fe6-30946254a5f7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        Facility Facility { get; set; }


        #region Allors
        [Id("e4ca9708-8c0c-451a-b63c-126a96b2ad72")]
        [AssociationId("780fde92-4842-4366-afe8-09ef9bde95f6")]
        [RoleId("54113d74-1620-4eb6-8c16-50531af1be17")]
        #endregion
        [Required]
        [Size(256)]

        string ContainerDescription { get; set; }

    }
}