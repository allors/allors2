// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllorsRoleStore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Identity
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    public class AllorsRoleStore<TRole> :
        IQueryableRoleStore<TRole>
        //IRoleClaimStore<TRole>
        where TRole : IdentityRole
    {
        public void Dispose()
        {
        }

        public Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        public IQueryable<TRole> Roles { get; }
    }
}
