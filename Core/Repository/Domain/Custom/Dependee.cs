// <copyright file="Dependee.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("2cc9bde1-80da-4159-bb20-219074266101")]
    #endregion
    public partial class Dependee : Object, DerivationCounted
    {
        #region inherited properties
        public int DerivationCount { get; set; }

        #endregion

        #region Allors
        [Id("1b8e0350-c446-48dc-85c0-71130cc1490e")]
        [AssociationId("97c6a03f-f0c7-4c7d-b40f-1353e34431bd")]
        [RoleId("89b8f5f6-5589-42ad-ac9e-1d984c02f7ea")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        #endregion
        public Subdependee Subdependee { get; set; }

        #region Allors
        [Id("c1e86449-e5a8-4911-97c7-b03de9142f98")]
        [AssociationId("2786b8ca-2d71-44cc-8e1e-1896ac5e6c5c")]
        [RoleId("af75f294-b20d-4304-8804-32ef9c0a324a")]
        #endregion
        public int Subcounter { get; set; }

        #region Allors
        [Id("d58d1f28-3abd-4294-abde-885bdd16f466")]
        [AssociationId("9a867244-8ea3-402b-9a9c-a78727dbee78")]
        [RoleId("5f570211-688e-4050-bf54-997d22a529d5")]
        #endregion
        public int Counter { get; set; }

        #region Allors
        [Id("e73b8fc5-0148-486a-9379-cfb051b303d2")]
        [AssociationId("db615c1c-3d08-4faa-b19f-740bd7102fbd")]
        [RoleId("bde110ae-8242-4d98-bdc3-feeed8fde742")]
        #endregion
        public bool DeleteDependent { get; set; }

        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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
