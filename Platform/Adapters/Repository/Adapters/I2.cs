namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0")]
    #endregion
	public partial interface I2 : Object, S1234, S2 
    {
        #region Allors
        [Id("35040d7c-ab7f-4a99-9d09-e01e24ca3cb9")]
        [AssociationId("3aa841fd-a95d-4ddc-b994-5e432fd9f2ef")]
        [RoleId("c39a79f1-3b54-45bb-ad24-3cec889691fc")]
        #endregion
        bool I2AllorsBoolean { get; set; }


        #region Allors
        [Id("4f095abd-8803-4610-87f0-2847ddd5e9f4")]
        [AssociationId("e1a86fa0-c857-4be0-8abc-704339bbdc82")]
        [RoleId("c7cb9a8b-7df5-4677-902f-b6f4b9aec802")]
        [Precision(19)]
        [Scale(2)]
        #endregion
        decimal I2AllorsDecimal { get; set; }


        #region Allors
        [Id("81d9eb2f-55a7-4d1c-853d-4369eb691ba5")]
        [AssociationId("fa701a92-ee96-4194-8ea9-3da451b2c775")]
        [RoleId("f4c841cb-821e-4e9c-ab2a-dc56aa3234ab")]
        #endregion
        DateTime I2AllorsDateTime { get; set; }


        #region Allors
        [Id("9f91841c-f63f-4ffa-bee6-62e100f3cd15")]
        [AssociationId("8841f638-0522-46b6-a6cf-797548264f0d")]
        [RoleId("15ba5c39-5269-4f61-b595-7b8b6fcefe9a")]
        [Size(256)]
        #endregion
        string I2AllorsString { get; set; }


        #region Allors
        [Id("d30dd036-6d28-48df-873b-3a76da8c029e")]
        [AssociationId("ee50ff17-39d8-44f7-8d14-e63f4c822ed4")]
        [RoleId("25cb17ec-01e2-4658-a06b-2a620f152923")]
        #endregion
        int I2AllorsInteger { get; set; }


        #region Allors
        [Id("fbad33e7-ede1-41fc-97e9-ddf33a0f6459")]
        [AssociationId("a9f79b82-bb7c-4cdc-ac16-333a1b994387")]
        [RoleId("81acc49f-16c9-4677-80f4-c3e768a7b9e3")]
        #endregion
        double I2AllorsDouble { get; set; }

    }
}