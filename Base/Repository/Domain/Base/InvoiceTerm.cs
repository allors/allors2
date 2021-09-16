// <copyright file="InvoiceTerm.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("BB280499-092B-4929-BF9A-D6A55419B8C0")]
    #endregion
    public partial class InvoiceTerm : SalesTerm, Object
    {
        #region inherited properties

        public string TermValue { get; set; }

        public TermType TermType { get; set; }

        public string Description { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

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
