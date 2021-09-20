// <copyright file="IProcedureInput.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database
{
    using System;
    using System.Collections.Generic;

    public interface IProcedureInput
    {
        IDictionary<string, IObject[]> Collections { get; }

        IDictionary<string, IObject> Objects { get; }

        IDictionary<string, string> Values { get; }

        public T[] GetCollection<T>();

        public T[] GetCollection<T>(string key);

        public T GetObject<T>() where T : class, IObject;

        public T GetObject<T>(string key) where T : class, IObject;

        public string GetValue(string key);

        public byte[] GetValueAsBinary(string key);

        public bool GetValueAsBoolean(string key);

        public double? GetValueAsFloat(string key);

        public int? GetValueAsInteger(string key);

        public DateTime? GetValueAsDateTime(string key);

        public decimal? GetValueAsDecimal(string key);

        public Guid? GetValueAsUnique(string key);
    }
}
