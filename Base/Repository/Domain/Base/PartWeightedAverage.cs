// <copyright file="PartWeightedAverage.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("e76a0a2b-6b19-4696-9b03-fc759ac60c8b")]
    #endregion
    public partial class PartWeightedAverage : Object
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("205b040c-1acc-4fb4-98e0-6ffc124ca89f")]
        [AssociationId("6678da7a-3069-4ee8-8eae-310d5f9b7069")]
        [RoleId("09085522-0ec8-4ce0-b929-b0e33c790067")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal AverageCost { get; set; }

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
