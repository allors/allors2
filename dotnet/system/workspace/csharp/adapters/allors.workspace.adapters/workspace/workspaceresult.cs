// <copyright file="Workspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class WorkspaceResult : IWorkspaceResult
    {
        private IList<IObject> mergeErrors;

        public bool HasErrors => this.VersionErrors?.Any() == true || this.MissingErrors?.Any() == true || this.mergeErrors != null;

        public IEnumerable<IObject> VersionErrors { get; }

        public IEnumerable<IObject> MissingErrors { get; }

        public IEnumerable<IObject> MergeErrors => this.mergeErrors ?? Array.Empty<IObject>();

        public void AddMergeError(IObject @Object)
        {
            this.mergeErrors ??= new List<IObject>();
            this.mergeErrors.Add(@Object);
        }
    }
}
