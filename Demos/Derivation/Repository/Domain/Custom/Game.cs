namespace Allors.Repository
{
    using Attributes;
    using System;

    #region Allors
    [Id("4c5f597f-7f75-4f4d-888a-1d21ebc9596a")]
    #endregion
    public partial class Game : Object
    {
        #region Inherited Properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        #endregion


        #region Allors

        [Id("30feae98-41ef-462f-a704-e70558a64df0")]
        [AssociationId("6625a67b-8ec5-4d55-a7b5-ae0bcac67914")]
        [RoleId("f48ba3dd-3108-4a54-934e-de8bef984145")]

        #endregion Allors
        [Workspace]
        public DateTime StartDate { get; set; }

        #region Allors

        [Id("73576236-f86a-4e74-b69f-dc1eec9f9213")]
        [AssociationId("944245af-ab3e-4fe4-b3fc-0c998f0a9ecf")]
        [RoleId("20e7c11c-4da1-4679-acea-64efadfdaf04")]

        #endregion Allors
        [Workspace]
        public DateTime EndDate { get; set; }

        #region Allors

        [Id("23fb081e-20bd-4af8-8cc9-418a109b2e80")]
        [AssociationId("85de8bfd-4235-45ca-b328-aae5df8a80c3")]
        [RoleId("5916b7e1-c2d8-4986-ae52-7bb0dc277d42")]

        #endregion Allors
        [Multiplicity(Multiplicity.OneToMany)]
        [Synced]
        [Workspace]
        public Score[] Scores { get; set; }

        #region Allors

        [Id("d0c95831-bc8c-4834-8b24-286886285afc")]
        [AssociationId("64869ba2-6087-430d-92fe-fc84961c8bf4")]
        [RoleId("5e875f28-fe42-4f8d-92a6-311035b6e32a")]

        #endregion Allors
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public GameType GameType { get; set; }

        #region Allors

        [Id("94f1308a-391a-4d06-bb19-5abaf9ee8388")]
        [AssociationId("0bc7908f-bb8f-4848-a2cf-c499650f24d6")]
        [RoleId("aa52ddc5-7c79-4a53-b5f8-e929bb4a4d37")]

        #endregion Allors
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public Person[] Winners { get; set; }

        #region Allors

        [Id("1803100c-f5f0-468d-8f0b-090fb46982f7")]
        [AssociationId("1e6489c7-adc3-4586-a71b-b49d86805526")]
        [RoleId("64ad4662-ee4b-439b-b176-a40b2e7cd9ad")]

        #endregion Allors
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public Person[] Declarers { get; set; }

        #region Allors

        [Id("375d3881-d54f-4692-b2db-8864669103f4")]
        [AssociationId("e2d2d33b-3be1-49a2-98c8-f4536fa1518a")]
        [RoleId("0e46bdc3-8807-48a5-bf39-1488e0a913d8")]

        #endregion Allors
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        [Derived]
        public Person[] Defenders { get; set; }

        #region Allors

        [Id("99ecbe80-b9b2-43a4-b9a3-7cd2e74e8182")]
        [AssociationId("1fd8b46e-df1c-4b60-94b4-89a99e2a2dd3")]
        [RoleId("2419b7eb-a559-4901-9f5d-4c5386b1164d")]

        #endregion Allors
        [Workspace]
        public int Overslagen { get; set; }

        #region inherited methods

        public void OnBuild()
        {

        }

        public void OnDerive()
        {

        }

        public void OnInit()
        {

        }

        public void OnPostBuild()
        {

        }

        public void OnPostDerive()
        {

        }

        public void OnPreDerive()
        {

        }

        #endregion inherited methods
    }
}