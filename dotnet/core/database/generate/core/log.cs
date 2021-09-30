// -------------------------------------------------------------------------------------------------
// <copyright file="Log.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Log type.</summary>
// -------------------------------------------------------------------------------------------------
namespace Allors.Development.Repository
{
    public abstract class Log
    {
        public bool ErrorOccured { get; protected set; }

        /// <summary>
        /// Log error messages.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        public abstract void Error(object sender, string message);
    }
}
