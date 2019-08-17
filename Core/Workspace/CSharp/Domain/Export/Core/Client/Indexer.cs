// <copyright file="Indexer.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Client
{
    using System.Collections.Generic;

    public class Indexer<T>
    {
        private readonly Dictionary<string, T> dictionary = new Dictionary<string, T>();

        public Indexer(Dictionary<string, T> dictionary) => this.dictionary = dictionary;

        public T this[string index] => this.dictionary.TryGetValue(index, out var value) ? value : default(T);
    }
}
