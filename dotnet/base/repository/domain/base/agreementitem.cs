// <copyright file="AgreementItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("8ba98e1b-1d4d-46b1-bf27-bb2bf53501fd")]
    #endregion
    public partial interface AgreementItem : DelegatedAccessControlledObject
    {
        #region Allors
        [Id("3ad6eaac-8cc3-4738-9a5b-617386e296c8")]
        [AssociationId("caf115e0-d7b8-43af-a183-f4df4c573e2c")]
        [RoleId("34bb4854-7f4f-4b27-8dd7-a2b9cbcf2331")]
        #endregion
        [Size(-1)]
        string Text { get; set; }

        #region Allors
        [Id("49d6363c-6006-4850-8a96-d87b9336ae59")]
        [AssociationId("aab2420d-b105-4328-a71c-c2cdce2712a3")]
        [RoleId("16e3ac4d-1641-417f-bb72-1167ae809ef9")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        Addendum[] Addenda { get; set; }

        #region Allors
        [Id("4bd76d2c-383e-460f-a597-4283addcbef8")]
        [AssociationId("a8ea8f52-bbe2-4723-85c6-8277904c3d93")]
        [RoleId("894d0335-40d3-4476-a87f-c3f11021862e")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        AgreementItem[] Children { get; set; }

        #region Allors
        [Id("9431dbfa-c620-445a-914f-8f12d4734b8b")]
        [AssociationId("b31b913d-82cc-418d-8167-df26ce483473")]
        [RoleId("c883859c-f67b-482e-b6f1-0a81fae1d927")]
        #endregion
        [Required]
        [Size(-1)]
        string Description { get; set; }

        #region Allors
        [Id("cfa9c54b-4e9f-4bd2-897d-baf8fb32fa9c")]
        [AssociationId("08518361-7f3d-4a44-9377-f407d6946668")]
        [RoleId("d4becca2-e702-42f8-beb7-f652e086ce83")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        AgreementTerm[] AgreementTerms { get; set; }
    }
}
