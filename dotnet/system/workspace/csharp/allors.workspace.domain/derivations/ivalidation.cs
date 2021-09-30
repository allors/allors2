// <copyright file="IDerivation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDerivation type.</summary>

namespace Allors.Workspace.Derivations
{
    using System.Collections.Generic;

    public interface IValidation
    {
        bool HasErrors { get; }

        IEnumerable<string> Errors { get; }

        void AddError(string error);
    }
}
