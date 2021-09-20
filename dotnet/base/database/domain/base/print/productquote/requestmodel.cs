// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.Print.ProductQuoteModel
{
    using System.Collections.Generic;

    public class RequestModel
    {
        public RequestModel(Quote quote, Dictionary<string, byte[]> imageByImageName)
        {
            this.Number = quote.Request?.RequestNumber;
        }

        public string Number { get; }
    }
}
