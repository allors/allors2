// <copyright file="UnitIds.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the UnitTypeTags type.</summary>

namespace Allors
{
    using System;

    /// <summary>
    /// The ids for unit ObjectTypes.
    /// Ids can be used for long term reference and should therefore never be changed.
    /// </summary>
    public static class UnitIds
    {
        /// <summary>
        /// The id of the binary type.
        /// </summary>
        public static readonly Guid Binary = new Guid("c28e515b-cae8-4d6b-95bf-062aec8042fc");

        /// <summary>
        /// The id of the boolean type.
        /// </summary>
        public static readonly Guid Boolean = new Guid("b5ee6cea-4E2b-498e-a5dd-24671d896477");

        /// <summary>
        /// The id of the date time type.
        /// </summary>
        public static readonly Guid DateTime = new Guid("c4c09343-61d3-418c-ade2-fe6fd588f128");

        /// <summary>
        /// The id of the decimal type.
        /// </summary>
        public static readonly Guid Decimal = new Guid("da866d8e-2c40-41a8-ae5b-5f6dae0b89c8");

        /// <summary>
        /// The id of the float type.
        /// </summary>
        public static readonly Guid Float = new Guid("ffcabd07-f35f-4083-bef6-f6c47970ca5d");

        /// <summary>
        /// The id of the integer type.
        /// </summary>
        public static readonly Guid Integer = new Guid("ccd6f134-26de-4103-bff9-a37ec3e997a3");

        /// <summary>
        /// The id of the string type.
        /// </summary>
        public static readonly Guid String = new Guid("ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9");

        /// <summary>
        /// The id of the unique type.
        /// </summary>
        public static readonly Guid Unique = new Guid("6dc0a1a8-88a4-4614-adb4-92dd3d017c0e");
    }
}
