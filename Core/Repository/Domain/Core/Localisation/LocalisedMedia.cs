// <copyright file="LocalisedMedia.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("2288E1F3-5DC5-458B-9F5E-076F133890C0")]
    #endregion
    public partial class LocalisedMedia : Localised, Deletable
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Locale Locale { get; set; }

        #endregion

        #region Allors
        [Id("B6AE19AE-76BF-4B84-9CBE-176217D94B9E")]
        [AssociationId("4C96A64D-6496-412D-915C-77FA9D4DA30E")]
        [RoleId("37FDEDB2-4B49-401C-8C5D-D22143691220")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public Media Media { get; set; }

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
    }
}
