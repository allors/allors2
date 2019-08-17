// <copyright file="C4.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("20049a79-20c7-478b-a5ba-c54b1e615168")]
    #endregion
    public partial class C4 : System.Object, I4, I34
    {
        #region inherited properties

        public string Name { get; set; }

        public double S1234AllorsDouble { get; set; }

        public decimal S1234AllorsDecimal { get; set; }

        public int S1234AllorsInteger { get; set; }

        public S1234 S1234many2one { get; set; }

        public C2 S1234C2one2one { get; set; }

        public C2[] S1234C2many2manies { get; set; }

        public S1234[] S1234one2manies { get; set; }

        public C2[] S1234C2one2manies { get; set; }

        public S1234[] S1234many2manies { get; set; }

        public string ClassName { get; set; }

        public DateTime S1234AllorsDateTime { get; set; }

        public S1234 S1234one2one { get; set; }

        public C2 S1234C2many2one { get; set; }

        public string S1234AllorsString { get; set; }

        public bool S1234AllorsBoolean { get; set; }

        public decimal I34AllorsDecimal { get; set; }

        public bool I34AllorsBoolean { get; set; }

        public double I34AllorsDouble { get; set; }

        public int I34AllorsInteger { get; set; }

        public string I34AllorsString { get; set; }

        #endregion

        #region Allors
        [Id("9f24fc51-8568-4ffc-b47a-c5c317d00954")]
        [AssociationId("77d762d7-4676-4b02-8319-11600c4314f3")]
        [RoleId("6e74ef8d-d748-4142-8073-afbf5534c43f")]
        [Size(256)]
        #endregion
        public string C4AllorsString { get; set; }

        #region inherited methods
        #endregion

    }
}
