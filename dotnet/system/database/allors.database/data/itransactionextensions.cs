// <copyright file="ITransactionExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using System.Linq;

    public static class ITransactionExtensions
    {
        public static T[] Resolve<T>(this ITransaction transaction, IExtent extent, IArguments arguments = null) where T : IObject => extent.Build(transaction, arguments).Cast<T>().ToArray();
    }
}
