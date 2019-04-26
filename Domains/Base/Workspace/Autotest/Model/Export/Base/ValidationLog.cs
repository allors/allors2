//-------------------------------------------------------------------------------------------------
// <copyright file="ValidationLog.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>
// <summary>Defines the ValidationReport type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Autotest
{
    using System.Collections.Generic;

    public class ValidationLog
    {
        private readonly List<string> errors;

        internal ValidationLog()
        {
            this.errors = new List<string>();
        }

        public bool HasErrors => this.Errors.Length > 0;

        public string[] Errors => this.errors.ToArray();

        public void AddError(string message)
        {
            this.errors.Add(message);
        }
    }
}