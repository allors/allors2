// <copyright file="ObjectType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta
{
    using System;

    public static partial class MetaIdentityExtensions
    {
        internal static void ValidateIdentity(this IMetaIdentifiableObject @this, ValidationLog validationLog)
        {
            if (@this.Id == Guid.Empty)
            {
                var message = "id on " + @this + " is required";
                validationLog.AddError(message, @this, ValidationKind.Unique, "IMetaObject.Id");
            }
            else if (validationLog.ExistId(@this.Id))
            {
                var message = "id " + @this + " is already in use";
                validationLog.AddError(message, @this, ValidationKind.Unique, "IMetaObject.Id");
            }
            else
            {
                validationLog.AddId(@this.Id);
            }
        }
    }
}
