// <copyright file="PayrollPreference.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("92f48c0c-31d9-4ed5-8f92-753de6af471a")]
    #endregion
    public partial class PayrollPreference : Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("2cb969f7-6415-4d5b-be55-7e691c2254e1")]
        [AssociationId("c2040e80-7608-4b9d-8e1e-c244b7155a81")]
        [RoleId("6f5e623e-b365-402c-aea0-09a386bb0377")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Percentage { get; set; }

        #region Allors
        [Id("802de3ea-0cb9-4815-bc56-497e75f487ae")]
        [AssociationId("75568752-3a42-412f-bf76-be6705bd441c")]
        [RoleId("fc4e22ba-ad4d-4f6e-b65f-d0aef6ff47ee")]
        #endregion
        [Size(256)]

        public string AccountNumber { get; set; }

        #region Allors
        [Id("a37e2763-6d8c-46c3-a69f-148458d2981b")]
        [AssociationId("4255cc8c-c97c-48a2-9111-f8658f478042")]
        [RoleId("7f79e26c-e5ef-45d4-88e9-8dcce8ffc2ba")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public PaymentMethod PaymentMethod { get; set; }

        #region Allors
        [Id("b576883f-0cfd-4973-aa49-479b6e712c75")]
        [AssociationId("f93aac27-8f9d-4b9e-a55d-5fad0efc6e86")]
        [RoleId("162fccce-f98a-4b2b-b840-72368a87b043")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public TimeFrequency Frequency { get; set; }

        #region Allors
        [Id("c71eb13a-8053-4d56-a3e3-dcd38a1e4f29")]
        [AssociationId("8955caa1-cfdb-4463-a6d2-80ce0f775470")]
        [RoleId("4851823e-72f4-4531-b505-bae6d70688e8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public DeductionType DeductionType { get; set; }

        #region Allors
        [Id("ded05ab7-351b-4b05-9e0a-010e6b4fbd0f")]
        [AssociationId("feb46721-492d-4508-9d28-5b6496f517cd")]
        [RoleId("ddfd9dac-42d5-42d0-909a-ebf6c8869c73")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

    }
}
