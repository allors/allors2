// <copyright file="IPullResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;

    public interface IPullResult : IResult
    {
        IEnumerable<IObject> MergeErrors { get; }

        IDictionary<string, IObject[]> Collections { get; }

        IDictionary<string, IObject> Objects { get; }

        IDictionary<string, object> Values { get; }

        public T[] GetCollection<T>() where T : class, IObject;

        public T[] GetCollection<T>(string key) where T : class, IObject;

        public T GetObject<T>() where T : class, IObject;

        public T GetObject<T>(string key) where T : class, IObject;

        public object GetValue(string key);

        public T GetValue<T>(string key);
    }
}
