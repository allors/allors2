// <copyright file="ProductType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("6451E06E-747E-4F58-98F5-2F9DC5D787B5")]
    #endregion
    public partial class ProductType : UniquelyIdentifiable, Deletable, Object
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("C7B92729-6744-4F29-BF55-FAA7FF783393")]
        [AssociationId("1DAECAD8-0A97-47DE-B579-642916B49AD2")]
        [RoleId("C14633D9-2081-4886-9C29-B5118F8E266C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public SerialisedItemCharacteristicType[] SerialisedItemCharacteristicTypes { get; set; }

        #region Allors
        [Id("B1DFD523-A9A4-4B7B-BEC4-4EAF107E196C")]
        [AssociationId("B71EBA52-CAB4-4ABB-B92B-7410EBFA0992")]
        [RoleId("46A5318D-6D4C-4E6E-84FB-32312BEC9873")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string Name { get; set; }

        #region inherited methods

        public void OnBuild()
        {
        }

        public void OnPostBuild()
        {
        }

        public void OnInit()
        {
        }

        public void OnPreDerive()
        {
        }

        public void OnDerive()
        {
        }

        public void OnPostDerive()
        {
        }

        public void Delete()
        {
        }

        #endregion
    }
}
