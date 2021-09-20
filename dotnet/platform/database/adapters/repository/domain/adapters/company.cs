// <copyright file="Company.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("b1b6361e-5ee5-434c-9c92-46c6166195c4")]
    #endregion
    public partial class Company : System.Object, Named
    {
        #region inherited properties
        public string Name { get; set; }

        public int Index { get; set; }

        #endregion

        #region Allors
        [Id("08ab248d-bdb1-49c5-a2da-d6485f49239f")]
        [AssociationId("ad0ef93c-be37-48a6-97c6-fd252d66fbac")]
        [RoleId("f2b72cff-9208-41c0-9c5c-09ed0725f107")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        public Person Manager { get; set; }

        #region Allors
        [Id("1a4087de-f116-4f79-9441-31faee8054f3")]
        [AssociationId("9c0ec4ba-9ef4-4d82-a94f-4984808c47cd")]
        [RoleId("978f3cdd-55d9-4086-b448-c313731604d8")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public Person[] Employees { get; set; }

        #region Allors
        [Id("28021756-f15f-4671-aa01-a40d3707d61a")]
        [AssociationId("eb38cf15-c545-4aa1-995b-f9d60508b87d")]
        [RoleId("1189280f-7a02-4f8d-b524-69e5728e03de")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        public Person FirstPerson { get; set; }

        #region Allors
        [Id("2f9fc05e-c904-4056-83f0-a7081762594a")]
        [AssociationId("b16d4eb4-1e3a-45d7-8c46-2b8bf8b5bc3f")]
        [RoleId("afea2d18-06e5-48e5-9fb9-3fd1daf65caf")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public Named[] NamedsOneSort2 { get; set; }

        #region Allors
        [Id("44abca14-9fb2-42a7-b8ab-a1ca87d87b2e")]
        [AssociationId("c212abeb-9a22-4577-98c2-3792ddb20ad9")]
        [RoleId("480debbc-ddb8-467d-bc3a-062a5c452b9f")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        #endregion
        public Person[] Owners { get; set; }

        #region Allors
        [Id("509c5341-3d87-4da4-a807-5567d897169b")]
        [AssociationId("f769d260-4f19-44fd-8986-34f29d395bb1")]
        [RoleId("eba712df-716d-4c2d-aeaf-d877a25a4d0b")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        #endregion
        public Person[] IndexedMany2ManyPersons { get; set; }

        #region Allors
        [Id("62b4ddac-efd7-4fc9-bbed-91c831a62f01")]
        [AssociationId("64358266-9014-4aa4-a34f-7c6cf2e87e5c")]
        [RoleId("0e47b601-ac16-4873-9544-07fb9404785b")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public Person[] PersonsOneSort1 { get; set; }

        #region Allors
        [Id("64c1be0a-0636-4da0-8404-2a93ab600cd9")]
        [AssociationId("f27225cb-2326-4997-b730-d280d6279d06")]
        [RoleId("70292538-4116-48c0-89bc-ca8185e4e253")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public Person[] PersonsManySort1 { get; set; }

        #region Allors
        [Id("996d27ff-3615-4a51-9214-944fac566a11")]
        [AssociationId("63ba9dd7-8a6c-4072-a6bb-b6f7229b90a7")]
        [RoleId("1c2a03e6-30f3-4257-bc6c-744b01d3a264")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public Named[] NamedsManySort1 { get; set; }

        #region Allors
        [Id("a9f60154-6bd1-4c76-94eb-edfd5beb6749")]
        [AssociationId("268a1cbf-d0d8-42da-a5e3-fe55a139bdfd")]
        [RoleId("18d5ae26-2d4c-452b-9b8b-f78fc30cf6b8")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public Person[] PersonsManySort2 { get; set; }

        #region Allors
        [Id("bdf71d38-8082-4a99-9636-4f4ec26fd45c")]
        [AssociationId("06c013e8-e053-40db-b39e-6dc2ba4ec634")]
        [RoleId("7ede6afc-ded3-453c-bd4d-5bc7034ba7d0")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public Person[] PersonsOneSort2 { get; set; }

        #region Allors
        [Id("c1f68661-4999-4851-9224-1878258b6a58")]
        [AssociationId("2923d509-2017-4906-80ab-058bc389eebf")]
        [RoleId("a1af343d-ea94-4433-893f-561d76a8aa7f")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        public Named NamedManySort2 { get; set; }

        #region Allors
        [Id("c53bdaea-c0a5-4179-bfbb-e12de45e2ae0")]
        [AssociationId("d9b8505c-48e0-4012-9f8a-623f18f8cd3b")]
        [RoleId("45e1bb36-5d7c-43dd-a889-2bcd6f225136")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public Person[] Many2ManyPersons { get; set; }

        #region Allors
        [Id("cde0a8e7-1a14-4f1a-a0ca-a305f0548df8")]
        [AssociationId("ba38ffe5-7075-4792-acb7-c5a07594a166")]
        [RoleId("7f9ab9a3-4296-4b3b-aa04-ab9e27d1f003")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public Company[] Children { get; set; }

        #region Allors
        [Id("cdf04399-aa37-4ea2-9ac8-bf6d19884933")]
        [AssociationId("15a7a418-5cc5-44a6-90b1-034620c08763")]
        [RoleId("6112c9a2-a775-45b5-879a-9fad898e21ba")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public Named[] NamedsOneSort1 { get; set; }

        #region inherited methods
        public void InheritedDoIt()
        {
        }
        #endregion
    }
}
