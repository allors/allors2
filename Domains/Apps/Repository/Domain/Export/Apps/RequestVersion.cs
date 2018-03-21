namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("CB830374-2F89-4911-9A33-98CE902741A8")]
    #endregion
    public partial interface RequestVersion : Version
    {
        #region Allors
        [Id("649F4856-6B08-4AC1-B4CB-87A1CCAFAAF8")]
        [AssociationId("0CE4C7E7-80A7-4269-9680-44F2B532AEB2")]
        [RoleId("4FDB8BE7-F60C-46A6-BBE7-935D3ED622D3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        RequestState RequestState { get; set; }

        #region Allors
        [Id("80F4AD18-6905-4916-AAF6-4016948F1451")]
        [AssociationId("2352CB88-6E53-422F-B71C-F37DA8C88585")]
        [RoleId("242CC244-A0DD-4CA4-B42C-D11D49BA2940")]
        #endregion
        [Workspace]
        [Size(-1)]
        string InternalComment { get; set; }

        #region Allors
        [Id("B1A51DCA-AA9E-49F7-A257-C17D70A3FAEE")]
        [AssociationId("8B21DBF2-D3A5-441A-9BA6-E5A2010F80CE")]
        [RoleId("92AAE1BD-0EAA-4AC0-918D-A30E78F699B7")]
        #endregion
        [Size(256)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("3BF82A10-6C93-4EB3-A3DD-615D8189712B")]
        [AssociationId("DD54C799-839D-47F3-B8E4-E5420D0B4791")]
        [RoleId("1DFB0071-D069-4151-B97C-306C9FD3DBCE")]
        #endregion
        [Required]
        [Workspace]
        DateTime RequestDate { get; set; }

        #region Allors
        [Id("28B2A495-04AA-42E2-807C-3D455C80BEC6")]
        [AssociationId("5CCFAAC3-1AD2-4AA5-963B-48C655E35FF5")]
        [RoleId("4C9C366E-1BE9-4880-87AF-0E310ED3D77B")]
        #endregion
        [Workspace]
        DateTime RequiredResponseDate { get; set; }

        #region Allors
        [Id("A85396E8-4B3A-4B2F-AE61-EA8D87418DAD")]
        [AssociationId("56537839-E08D-443C-B1C7-9E884BF6BFEA")]
        [RoleId("3E7BDF03-58AF-497D-8E64-238608ACB110")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        RequestItem[] RequestItems { get; set; }

        #region Allors
        [Id("8CC34A50-82AB-44CE-BE9B-64691ACDDCE9")]
        [AssociationId("70DAD1CB-624D-4BF6-BF9B-A78D982F4BC7")]
        [RoleId("2304A96B-2994-4F16-92EB-198618915508")]
        #endregion
        [Size(256)]
        [Workspace]
        string RequestNumber { get; set; }

        #region Allors
        [Id("9ECA802A-F588-4241-BC4C-8083D56DFE1E")]
        [AssociationId("F7DC383A-075A-44C2-9460-49132540E551")]
        [RoleId("5420A2B3-59CF-443D-9A69-CF9D32206FD1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        RespondingParty[] RespondingParties { get; set; }

        #region Allors
        [Id("7A6D4128-55C4-4E48-A3A0-07EF25ACF85E")]
        [AssociationId("B2AC6D68-9B58-4EA4-B6F2-0FC539E97646")]
        [RoleId("4A1DEC44-0229-4E7D-A051-E2678E1877A0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Party Originator { get; set; }

        #region Allors
        [Id("A15D9E5E-2300-4618-B3F1-F572B1729231")]
        [AssociationId("27B04FA9-FC8B-4761-A1E7-549C6612B5A4")]
        [RoleId("F928DED6-AE1D-44B0-94CB-802F7EAD7C0B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Currency Currency { get; set; }

        #region Allors
        [Id("6ED70134-E2A9-4F73-A300-78DFC79F0004")]
        [AssociationId("A7192680-EB21-408C-B404-619FE562E8B7")]
        [RoleId("1584F649-9945-40F4-A4E2-3D8BC356DFC3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        ContactMechanism FullfillContactMechanism { get; set; }

        #region Allors
        [Id("8FCA340E-777B-4260-A6A9-8B7515D5FA5F")]
        [AssociationId("C591535B-4519-4D9C-9473-6FCAD4D38F89")]
        [RoleId("015DC4E9-523E-468F-AD96-E4768EC68BA3")]
        #endregion
        [Workspace]
        string EmailAddress { get; set; }

        #region Allors
        [Id("FEA30E94-84F3-4E2D-B894-EED0829BA7DD")]
        [AssociationId("61407EC8-8789-4DB4-92AF-BEFB1C99B5C1")]
        [RoleId("F6CE7907-2551-48F5-8285-6900A392FB9F")]
        #endregion
        [Workspace]
        string TelephoneNumber { get; set; }

        #region Allors
        [Id("CFCF2525-D88A-49B2-997F-4516BCF4364A")]
        [AssociationId("426CFEA4-D9B8-4E15-AEF1-1D469A79CC7F")]
        [RoleId("01F47852-430E-45C5-9179-AA4056F45059")]
        #endregion
        [Workspace]
        string TelephoneCountryCode { get; set; }
    }
}