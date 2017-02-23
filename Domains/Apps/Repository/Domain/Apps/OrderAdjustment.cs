namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("c5578565-c07a-4dc1-8381-41955db364e2")]
    #endregion
	public partial interface OrderAdjustment : AccessControlledObject 
    {


        #region Allors
        [Id("4e7cbdda-9f19-44dd-bbef-6cab5d92a8a3")]
        [AssociationId("5ccd492c-cf29-468b-b99d-126a9573e573")]
        [RoleId("7388d1a3-f24a-4c41-b57c-938160b3d1a6")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        decimal Amount { get; set; }


        #region Allors
        [Id("78d6de86-0f4d-4d8e-a9a6-4730668fa754")]
        [AssociationId("51d96df2-1e92-4ea2-8ec7-e918d5781ae7")]
        [RoleId("933a70e0-0fa0-42cd-a4d5-b3eb10b57802")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        VatRate VatRate { get; set; }


        #region Allors
        [Id("bc1ad594-88b6-4176-994c-a52be672f06d")]
        [AssociationId("ebc960bf-dd8c-4854-afec-185b260315e9")]
        [RoleId("9d2f66e2-0bbd-46ab-b65b-43e6b38383b9")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        decimal Percentage { get; set; }

    }
}