// <copyright file="TestFixture.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System;
    using System.Globalization;

    public class TestFixture : IDisposable
    {
        public TestFixture() => CultureInfo.CurrentCulture = new CultureInfo("nl-BE");

        public void Dispose()
        {
        }
    }
}
