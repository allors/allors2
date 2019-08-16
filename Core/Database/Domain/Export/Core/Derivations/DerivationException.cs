
// <copyright file="DerivationException.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Text;

    public class DerivationException : Exception
    {
        public DerivationException(IValidation validation) => this.Validation = validation;

        public IValidation Validation { get; private set; }

        public override string Message
        {
            get
            {
                var message = new StringBuilder();
                foreach (var error in this.Validation.Errors)
                {
                    message.Append(error.Message + "\n");
                }

                return message.ToString();
            }
        }
    }
}
