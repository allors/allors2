// <copyright file="Command.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System;

    public interface IReader : IDisposable
    {
        bool Read();

        object this[int i] { get; }

        object GetValue(int i);

        bool GetBoolean(int i);

        DateTime GetDateTime(int i);

        decimal GetDecimal(int i);

        double GetDouble(int i);

        int GetInt32(int i);

        long GetInt64(int i);

        string GetString(int i);

        Guid GetGuid(int i);

        bool IsDBNull(int i);
    }
}
