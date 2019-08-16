namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("3a5dcec7-308f-48c7-afee-35d38415aa0b")]
    #endregion
    public partial class Organisation : Object, Deletable, UniquelyIdentifiable
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("73f23588-1444-416d-b43c-b3384ca87bfc")]
        [AssociationId("d1a098bf-a3d8-4b71-948f-a77ae82f02db")]
        [RoleId("a365f0ee-a94f-4435-a7b1-c92ac804a845")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        #endregion
        public Address[] Addresses { get; set; }

        #region Allors
        [Id("2cfea5d4-e893-4264-a966-a68716839acd")]
        [AssociationId("c3c93567-1d78-42ea-a8cf-77549cd1a235")]
        [RoleId("d5965473-66cd-44b2-8048-a521c9cdadd0")]
        [Size(-1)]
        #endregion
        public string Description { get; set; }

        #region Allors
        [Id("49b96f79-c33d-4847-8c64-d50a6adb4985")]
        [AssociationId("b031ef1a-0102-4b19-b85d-aa9c404596c3")]
        [RoleId("b95c7b34-a295-4600-82c8-826cc2186a00")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        #endregion
        public Person[] Employees { get; set; }

        #region Allors
        [Id("17e55fcd-2c82-462b-8e31-b4a515acdaa9")]
        [AssociationId("e6fc633a-de9d-42a5-af03-b2359b2c2ea4")]
        [RoleId("6ab3328a-0fe1-4e98-b10d-eee420a90ffb")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        #endregion
        public Media[] Images { get; set; }

        #region Allors
        [Id("5fa25b53-e2a7-44c8-b6ff-f9575abb911d")]
        [AssociationId("6a382c73-c6a2-4d8b-bc85-4623ede54298")]
        [RoleId("1c3dec18-978c-470a-8857-5210b9267185")]
        #endregion
        public bool Incorporated { get; set; }

        #region Allors
        [Id("7046c2b4-d458-4343-8446-d23d9c837c84")]
        [AssociationId("0671f523-a557-41e1-9d05-0e89d8d1ae2d")]
        [RoleId("c84a6696-a1e9-4794-86c3-50e1f009c845")]
        #endregion
        public DateTime IncorporationDate { get; set; }

        #region Allors
        [Id("01dd273f-cbca-4ee7-8c2d-827808aba481")]
        [AssociationId("ffc3b92f-860a-4e45-90e1-b9ba7ab27a27")]
        [RoleId("e567907e-ca61-4ec1-ab06-62dbb84e5d57")]
        [Indexed]
        [Size(-1)]
        #endregion
        public string Information { get; set; }

        #region Allors
        [Id("68c61cea-4e6e-4ed5-819b-7ec794a10870")]
        [AssociationId("8494ad76-3422-4799-b5a6-caa077e53aca")]
        [RoleId("90489246-8590-4578-8b8d-716a25abd27d")]
        #endregion
        public bool IsSupplier { get; set; }

        #region Allors
        [Id("b201d2a0-2335-47a1-aa8d-8416e89a9fec")]
        [AssociationId("e332003a-0287-4aab-9d95-257146ee4f1c")]
        [RoleId("b1f5b479-e4d0-46de-8ad4-347076d9f180")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public Media Logo { get; set; }

        #region Allors
        [Id("ddcea177-0ed9-4247-93d3-2090496c130c")]
        [AssociationId("944d024b-81eb-442f-8f50-387a588d2373")]
        [RoleId("2c3bc00d-6715-4c1b-be78-753f7f306df0")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public Address MainAddress { get; set; }

        #region Allors
        [Id("dbef262d-7184-4b98-8f1f-cf04e884bb92")]
        [AssociationId("ed76a631-00c4-4753-b3d4-b3a53b9ecf4a")]
        [RoleId("19de0627-fb1c-4f55-9b65-31d8008d0a48")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public Person Manager { get; set; }

        #region Allors
        [Id("2cc74901-cda5-4185-bcd8-d51c745a8437")]
        [AssociationId("896a4589-4caf-4cd2-8365-c4200b12f519")]
        [RoleId("baa30557-79ff-406d-b374-9d32519b2de7")]
        [Indexed]
        [Size(256)]
        [Workspace]
        #endregion
        public string Name { get; set; }

        #region Allors
        [Id("845ff004-516f-4ad5-9870-3d0e966a9f7d")]
        [AssociationId("3820f65f-0e79-4f30-a973-5d17dca6ad33")]
        [RoleId("58d7df91-fbc5-4bcb-9398-a9957949402b")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Person Owner { get; set; }

        #region Allors
        [Id("15f33fa4-c878-45a0-b40c-c5214bce350b")]
        [AssociationId("4fdd9abb-f2e7-4f07-860e-27b4207224bd")]
        [RoleId("45bef644-dfcf-417a-9356-3c1cfbcada1b")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public Person[] Shareholders { get; set; }

        #region Allors
        [Id("bac702b8-7874-45c3-a410-102e1caea4a7")]
        [AssociationId("8c2ce648-3942-4ead-9772-308c29bc905e")]
        [RoleId("26a60588-3c90-4f4e-9bb6-8f45fe8f9606")]
        [Size(256)]
        #endregion
        public string Size { get; set; }

        #region Allors
        [Id("D3DB6E8C-9C10-47BA-92B1-45F5DDFFA5CC")]
        [AssociationId("4955AC7F-F840-4F24-B44C-C2D3937D2D44")]
        [RoleId("9033AE73-83F6-4529-9F81-84FD9D35D597")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Person CycleOne { get; set; }

        #region Allors
        [Id("C6CCA1C5-5799-4517-87F5-095DA0EEEC64")]
        [AssociationId("6ABCD4E2-44A7-46B4-BD98-D052F38B7C50")]
        [RoleId("E01ACE3C-2314-477C-8997-14266D9711E0")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public Person[] CycleMany { get; set; }

        #region Allors
        [Id("607C1D85-E722-40BC-A4D6-0C6A7244AF6A")]
        [AssociationId("1AFD34A4-F075-4034-92D9-85EDDC6998D2")]
        [RoleId("FB4834EB-344E-46ED-8D75-BF0C442C7078")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Data OneData { get; set; }

        #region Allors
        [Id("897DA15E-C250-441F-8F5C-6F9F3E7870EB")]
        [AssociationId("3B9B5811-C034-45A1-91DD-2A7C11FC5EC2")]
        [RoleId("658A5F21-58F2-413F-BEA0-DE9C3F1F8AB0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public Data[] ManyDatas { get; set; }

        #region inherited methods
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }
        #endregion

        [Id("1869873F-F2F0-4D03-A0F9-7DC73491C117")]
        [Workspace]
        public void JustDoIt() { }

        [Id("2CD2FF48-93FC-4C7D-BF2F-3F411D0DF7C3")]
        [Workspace]
        public void ToggleCanWrite() { }
    }
}
