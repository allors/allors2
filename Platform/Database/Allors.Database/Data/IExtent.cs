// <copyright file="IExtent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Meta;

    public interface IExtent
    {
        IComposite ObjectType { get; }

        Sort[] Sorting { get; set; }

        Allors.Extent Build(ISession session, IDictionary<string, string> parameters = null);

        Protocol.Data.Extent Save();

        bool HasMissingArguments(IDictionary<string, string> parameters);
    }
}
