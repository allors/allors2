// <copyright file="PostalCodes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class PostalCodes
    {
        private Sticky<string, PostalCode> postalCodeByCode;

        public Sticky<string, PostalCode> PostalCodeByCode => this.postalCodeByCode ??= new Sticky<string, PostalCode>(this.Session, M.PostalCode.Code);
    }
}
