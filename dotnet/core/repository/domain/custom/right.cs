﻿// <copyright file="Right.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region

    [Id("E4BC4E69-831C-4D9B-93D9-531D226819E1")]

    #endregion
    public partial class Right : DerivationCounted
    {
        #region inherited properties
        public int DerivationCount { get; set; }

        #endregion

        #region Allors
        [Id("658FE4F7-FC40-4B3A-ABB1-84723E66F20C")]
        [AssociationId("6E4C2718-CABB-497A-93AC-CF6BDF3504B3")]
        [RoleId("ACA018A3-F727-48D7-A43A-D6664C3326EB")]
        #endregion
        [Required]
        public int Counter { get; set; }

        #region inherited methods

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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

        #endregion
    }
}
