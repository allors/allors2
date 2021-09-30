// <copyright file="IProcedureOutput.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database
{
    using System.Collections.Generic;
    using Data;

    public interface IProcedureOutput
    {
        void AddCollection(string name, in IEnumerable<IObject> collection, Node[] tree = null);

        void AddObject(string name, IObject @object, Node[] tree = null);

        void AddValue(string name, object value);
    }
}
