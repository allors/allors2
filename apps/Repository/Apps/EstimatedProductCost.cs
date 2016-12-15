namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("c8df7ac5-4e6f-4add-981f-f0d9a8c14e24")]
    #endregion
	public partial interface EstimatedProductCost : Period, AccessControlledObject 
    {


        #region Allors
        [Id("2a8f919f-19f0-4b33-b8b8-26937d49d298")]
        [AssociationId("6d46215f-6af1-49b9-bc27-41de412a5b43")]
        [RoleId("0d0adab4-db9a-492b-8aaf-e40b864705aa")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal Cost { get; set; }


        #region Allors
        [Id("78a7ee9c-4aeb-471d-ae17-5878737f1f67")]
        [AssociationId("d4e26be2-9adc-4ded-b373-e88c7ecd7e29")]
        [RoleId("51bb9283-5e98-4a69-ae20-85460ee532d7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        Currency Currency { get; set; }


        #region Allors
        [Id("ce0f4392-cf76-49ba-a6bd-47b4e125ec61")]
        [AssociationId("acc9ae9a-8cb4-46cc-a507-db82759435d8")]
        [RoleId("5ebf8530-9a22-43d0-a1db-d976dfcbeaea")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        Organisation Organisation { get; set; }


        #region Allors
        [Id("d5e63839-7009-4582-8d9a-ac9591aa10c9")]
        [AssociationId("bfc2363f-b9ef-43ba-b5de-83104b9492ba")]
        [RoleId("31982d33-6240-4718-b9db-6762adb85670")]
        #endregion
        [Size(256)]

        string Description { get; set; }


        #region Allors
        [Id("e7942246-0343-437e-9b92-fc2d5e6438fd")]
        [AssociationId("434c6b12-146d-4f53-b1a3-5b75afaf57f2")]
        [RoleId("c763f72b-aa80-4caa-91b0-eddb949d3d34")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        GeographicBoundary GeographicBoundary { get; set; }

    }
}