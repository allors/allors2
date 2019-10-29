// <copyright file="AllorsRoleStore.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Security
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    public class AllorsRoleStore :
            IQueryableRoleStore<IdentityRole>
        // IRoleClaimStore<IdentityRole>
    {
        public void Dispose()
        {
        }

        public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public IQueryable<IdentityRole> Roles { get; }
    }
}
