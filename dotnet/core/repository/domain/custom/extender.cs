// <copyright file="Extender.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("830cdcb1-31f1-4481-8399-00c034661450")]
    #endregion
    public partial class Extender : Object
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("525bbc9e-d488-419f-ac02-0ab6ac409bac")]
        [AssociationId("7dcdf3d7-25ad-4e8f-9634-63b771990681")]
        [RoleId("bf9f7482-5277-40db-a6ac-5d4731cb5537")]
        [Size(256)]
        #endregion
        public string AllorsString { get; set; }

        #region inherited methods

        public Restriction[] Restrictions { get; set; }

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
