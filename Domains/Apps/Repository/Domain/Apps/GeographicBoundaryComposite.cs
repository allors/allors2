namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("3b7ac95a-fdab-488d-b599-17ef9fcf33b0")]
    #endregion
	public partial interface GeographicBoundaryComposite : GeographicBoundary 
    {


        #region Allors
        [Id("77d5f129-6096-45da-8b9f-39ef19276f1d")]
        [AssociationId("7484e00e-de39-4fbe-981a-aff3e693cf89")]
        [RoleId("03ef822a-e2d3-43ba-9051-2c663593fb31")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]

        GeographicBoundary[] Associations { get; set; }

    }
}