// <copyright file="Rounder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public static partial class Rounder
    {
        public static decimal RoundDecimal(decimal value, int digits) => Math.Round(value, digits, MidpointRounding.AwayFromZero);
    }
}
