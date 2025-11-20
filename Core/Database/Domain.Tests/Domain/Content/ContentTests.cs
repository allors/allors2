// <copyright file="ContentTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ContentTests type.</summary>

namespace Tests
{
    public abstract class ContentTests : DomainTest
    {
        protected static byte[] GetByteArray() => GetByteArray("Some string");

        protected static byte[] GetByteArray(string v) => System.Text.Encoding.UTF8.GetBytes(v);
    }
}
