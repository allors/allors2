//-------------------------------------------------------------------------------------------------
// <copyright file="IExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Meta;

    public interface IExtent
    {
        IComposite ObjectType { get; }

        Sort[] Sorting { get; set; }

        Extent Build(ISession session, IReadOnlyDictionary<string, object> arguments = null);

        Protocol.Data.Extent Save();

        bool HasMissingArguments(IReadOnlyDictionary<string, object> arguments);
    }
}
