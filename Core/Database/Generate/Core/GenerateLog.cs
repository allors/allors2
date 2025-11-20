// <copyright file="GenerateLog.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Development.Repository.Tasks
{
    using System;

    internal class GenerateLog : Log
    {
        public GenerateLog() => this.ErrorOccured = false;

        public override void Error(object sender, string message)
        {
            this.ErrorOccured = true;
            Console.WriteLine(message);
        }
    }
}
