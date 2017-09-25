namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("96fe3000-606e-4f88-ba04-87544ef176ca")]
    #endregion
    public partial class PartyPackageRevenue : AccessControlledObject, Deletable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("3fc82b94-ce74-42d7-91d8-e97a79117f4f")]
        [AssociationId("917ecd65-8097-4e6b-93ce-662b18ccf424")]
        [RoleId("33bf1c47-0d26-43e4-841d-bb5d85da1bdf")]
        #endregion
        [Required]

        public int Month { get; set; }
        #region Allors
        [Id("646382fa-3794-46be-81a0-28a1609e65b0")]
        [AssociationId("0bb2b710-90c3-42c9-8eb4-5bffb06cb705")]
        [RoleId("0ec8beac-06c9-4f7f-a39b-fb9d7bfcae0f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Package Package { get; set; }
        #region Allors
        [Id("8896e95d-80e3-42dc-8ba7-ad3fdef665f9")]
        [AssociationId("5d061264-d9c8-471c-b5be-3251502e24e1")]
        [RoleId("d1757181-9a09-4075-99ac-5c2a13ad85d3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Party Party { get; set; }
        #region Allors
        [Id("af3eeba0-867c-4484-b593-1815b38c8bf4")]
        [AssociationId("450fc3e5-f6ca-4f96-ab52-afb71421b6b5")]
        [RoleId("80615a8d-b2ff-4671-b9bb-0667413cd74c")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Revenue { get; set; }
        #region Allors
        [Id("b9648327-4521-4daf-b68f-52cd78095998")]
        [AssociationId("e65e0fb0-26a8-4640-ae6a-c8402889dc8e")]
        [RoleId("b3788737-41da-4480-8223-bc398e021561")]
        #endregion
        [Required]

        public int Year { get; set; }
        #region Allors
        [Id("e8384ea1-cb9d-43a0-8409-68dc86ca8def")]
        [AssociationId("f50f2ba1-9d0b-4eee-87d2-626fd89422c7")]
        [RoleId("bf38c186-30a1-40e5-90eb-a326865a2d19")]
        #endregion
        [Size(256)]

        public string PartyPackageName { get; set; }
        #region Allors
        [Id("e93042c4-1e6c-47c5-9004-f6ddcbfdbb33")]
        [AssociationId("39c71cdb-847a-474f-92b4-827e2eb95c22")]
        [RoleId("d498a399-c183-433c-8260-396c4e2b997d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Currency Currency { get; set; }

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