// -------------------------------------------------------------------------------------------------
// <copyright file="Log.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>
// <summary>Defines the Log type.</summary>
// -------------------------------------------------------------------------------------------------
namespace Allors.Development.Repository
{
    public abstract class Log
    {
        /// <summary>
        /// Log error messages.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        public abstract void Error(object sender, string message);

        public bool ErrorOccured { get; protected set; }
    }
}
