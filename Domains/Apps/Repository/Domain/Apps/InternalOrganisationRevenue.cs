namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("930565df-e12c-43c3-9679-a2b42d5a8782")]
    #endregion
    public partial class InternalOrganisationRevenue : AccessControlledObject, Deletable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("0f1c3ee2-de89-4828-982c-8168c9d8cf7c")]
        [AssociationId("ffbaa100-a74b-46c1-bc93-d84f48918d88")]
        [RoleId("2f8002ef-0f3b-46af-a930-cd426a2ee1a8")]
        #endregion
        [Required]

        public int Month { get; set; }
        #region Allors
        [Id("284b35b3-583b-4843-8f65-0abafc493eb7")]
        [AssociationId("d5e828bc-e39d-44d7-9e82-ab9471fd5d75")]
        [RoleId("d6d12ab5-4272-4095-b9ad-7222ec3071c1")]
        #endregion
        [Indexed]
        [Required]

        public int Year { get; set; }
        #region Allors
        [Id("5a982cc9-01c5-41f1-83ba-97747299205b")]
        [AssociationId("c0f705d6-4ad1-4e19-ae3c-3b12b4f2a6ec")]
        [RoleId("7a66c118-e965-430d-abbb-7a9c19a401e1")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Revenue { get; set; }
        #region Allors
        [Id("8e250efc-f571-4567-a747-cefe30118ddc")]
        [AssociationId("48e4e136-0006-4c83-8616-246fb432346e")]
        [RoleId("7e1086df-5289-48a5-8ee3-d8f14b39d4c7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Currency Currency { get; set; }
        #region Allors
        [Id("e618c592-229d-4135-b26b-d57a3d1802ac")]
        [AssociationId("10f7837f-9025-4284-8821-04fe9291c726")]
        [RoleId("c0de4d35-c58e-4477-866a-d018a7ea7c7c")]
        #endregion
        [Size(256)]

        public string PartyName { get; set; }
        #region Allors
        [Id("f1e4b78b-5166-46fc-8a9f-b009da84a3df")]
        [AssociationId("6e0495e8-cfac-49cb-89d4-7aae57b01aaa")]
        [RoleId("105af49d-ba84-457a-8a73-d6fcab787d38")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

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