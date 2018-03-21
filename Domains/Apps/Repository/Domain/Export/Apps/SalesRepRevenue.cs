namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("749e2a92-b397-4d36-b965-6073d45a4135")]
    #endregion
    public partial class SalesRepRevenue : AccessControlledObject, Deletable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("0bf9f020-7704-4e4e-92f6-06e747dc9463")]
        [AssociationId("7ca286bb-a26d-4b7a-bfbe-8305d885d035")]
        [RoleId("7cfa4dad-e2a4-4c95-b25f-476a8a2b7521")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Revenue { get; set; }
        #region Allors
        [Id("70b2fc04-ce4e-4af7-b287-02883fe660e9")]
        [AssociationId("48f05073-776f-4465-9763-ca71c785c058")]
        [RoleId("d5009f5b-b990-465d-b868-0bf977b33a4c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Currency Currency { get; set; }

        #region Allors
        [Id("89ff9736-f2d1-4609-ac99-b60f5b37f406")]
        [AssociationId("86e77422-4347-4e97-99ba-1c3b7cf57220")]
        [RoleId("5c19cc4e-4599-48b7-ab74-a1be3509317d")]
        #endregion
        [Required]

        public int Month { get; set; }
        #region Allors
        [Id("b1aa9e43-5ccc-4e1d-821e-39af02321a79")]
        [AssociationId("092108de-fb58-4fb2-b844-443fd476a383")]
        [RoleId("d1667908-4149-45dd-949e-ab00fbf3c7c4")]
        #endregion
        [Size(256)]

        public string SalesRepName { get; set; }
        #region Allors
        [Id("b72d2ab7-ad47-41dd-8dab-4b6364efc342")]
        [AssociationId("6160b809-a3a4-434a-b05a-cfdb1a3a1dd4")]
        [RoleId("6e712c98-649e-49cc-9484-0a0d407f02a7")]
        #endregion
        [Required]

        public int Year { get; set; }
        #region Allors
        [Id("be530b0c-6ab9-43a2-a974-f06015ae3480")]
        [AssociationId("cd786202-3d96-4f6b-94af-27ffb92608e3")]
        [RoleId("0bebaa7b-9332-44a6-abff-454175d2f2a5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Person SalesRep { get; set; }


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