//-------------------------------------------------------------------------------------------------
// <copyright file="PrefetchPolicyBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ISessionExtension type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors
{
    using Meta;

    public static partial class PrefetchPolicyBuilderExtensions
    {
        private static readonly PrefetchPolicy AccessControlPrefetchPolicy;

        private static readonly PrefetchPolicy SecurityTokenPrefetchPolicy;

        static PrefetchPolicyBuilderExtensions()
        {
            AccessControlPrefetchPolicy = new PrefetchPolicyBuilder()
                .WithRule(MetaAccessControl.Instance.CacheId.RoleType)
                .Build();

            SecurityTokenPrefetchPolicy = new PrefetchPolicyBuilder()
                .WithRule(MetaSecurityToken.Instance.AccessControls, AccessControlPrefetchPolicy)
                .Build();
        }

        public static void WithWorkspaceRules(this PrefetchPolicyBuilder @this, Class @class)
        {
            foreach (var roleType in @class.WorkspaceRoleTypes)
            {
                @this.WithRule(roleType);
            }
        }

        public static void WithSecurityRules(this PrefetchPolicyBuilder @this, Class @class)
        {
            if (@class.DelegatedAccessRoleTypes != null)
            {
                var builder = new PrefetchPolicyBuilder()
                    .WithRule(MetaObject.Instance.SecurityTokens, SecurityTokenPrefetchPolicy)
                    .WithRule(MetaObject.Instance.DeniedPermissions)
                    .Build();

                var delegatedAccessRoleTypes = @class.DelegatedAccessRoleTypes;
                foreach (var delegatedAccessRoleType in delegatedAccessRoleTypes)
                {
                    @this.WithRule(delegatedAccessRoleType, builder);
                }
            }

            @this.WithRule(MetaObject.Instance.SecurityTokens, SecurityTokenPrefetchPolicy);
            @this.WithRule(MetaObject.Instance.DeniedPermissions);
        }
    }
}
