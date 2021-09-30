// <copyright file="Model.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("273e69b7-6cda-44d4-b1d6-605b32a6a70d")]
    #endregion
    public partial class Model : Deletable, Object
    {
        #region inherited properties

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("ACB93740-E54D-40DD-BC61-579383AFBDC9")]
        [AssociationId("8A1FE510-5F10-453C-88C7-119DF24DA29C")]
        [RoleId("72811E1A-62A9-498B-8513-7079A459D23E")]
        [Required]
        [Workspace]
        #endregion
        public string Name { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete()
        {
        }

        #endregion
    }
}
