// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    public partial class AccessControl
    {
        public string DebuggerDisplay
        {
            get
            {
                var userNames = this.Subjects.Select(v => v.UserName);
                var groupNames = this.SubjectGroups.Select(v => v.Name);
                var names = string.Join(",", userNames.Union(groupNames));

                return $"{this.Role.Name}: {names} [{this.strategy.ObjectId}]";
            }
        }
    }
}
