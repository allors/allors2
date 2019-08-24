// <copyright file="RequestModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.ProductQuoteModel
{
    public class RequestModel
    {
        public RequestModel(Request request) => this.Number = request?.RequestNumber;

        public string Number { get; }
    }
}
