// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionedExtensions.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Allors.Meta;

    public static partial class VersionedExtensions
    {
        public static void BaseOnPostDerive(this Versioned @this, ObjectOnPostDerive method)
        {
            // TODO: move parts to Meta
            var derivation = method.Derivation;
            var versionedClass = (Class)@this.Strategy.Class;
            var metaPopulation = versionedClass.MetaPopulation;
            var versionClass = metaPopulation.FindByName(versionedClass.Name + "Version");
            var versionTypeRegex = new Regex(".+Version");

            var currentVersionRole = versionedClass.RoleTypes.First(v => v.Name.Equals("CurrentVersion"));
            var currentVersion = @this.Strategy.GetCompositeRole(currentVersionRole.RelationType);

            var isNewVersion = currentVersion == null;
            if (!isNewVersion)
            {
                foreach (var versionRoleType in versionClass.RoleTypes.Where(v => versionTypeRegex.IsMatch(v.AssociationType.ObjectType.Name)))
                {
                    var versionedRoleType = versionedClass.RoleTypes.FirstOrDefault(v => v.Name.Equals(versionRoleType.Name));
                    if (versionedRoleType == null)
                    {
                        throw new Exception("Could not find versioned role " + versionRoleType.Name);
                    }

                    if (versionRoleType.IsMany)
                    {
                        var versionedRole = @this.Strategy.GetCompositeRoles(versionedRoleType.RelationType);
                        var versionRole = currentVersion.Strategy.GetCompositeRoles(versionRoleType.RelationType);
                        if (!(versionedRole.Count == 0 && versionRole.Count == 0) ||
                            versionedRole.Count != versionRole.Count ||
                            !versionedRole.ToArray().OrderBy(s => s)
                                .SequenceEqual(versionRole.ToArray().OrderBy(t => t)))
                        {
                            isNewVersion = true;
                            break;
                        }
                    }
                    else
                    {
                        var versionedRole = @this.Strategy.GetRole(versionedRoleType.RelationType);
                        var versionRole = currentVersion.Strategy.GetRole(versionRoleType.RelationType);
                        if (!object.Equals(versionedRole, versionRole))
                        {
                            isNewVersion = true;
                            break;
                        }
                    }
                }
            }

            if (isNewVersion)
            {
                var session = @this.Strategy.Session;
                var newVersion = (Version)Allors.ObjectBuilder.Build(session, versionClass);
                newVersion.DerivationId = derivation.Id;
                newVersion.DerivationTimeStamp = derivation.TimeStamp;
                
                foreach (var versionRoleType in versionClass.RoleTypes.Where(v => versionTypeRegex.IsMatch(v.AssociationType.ObjectType.Name)))
                {
                    var versionedRoleType = versionedClass.RoleTypes.FirstOrDefault(v => v.Name.Equals(versionRoleType.Name));
                    if (versionedRoleType == null)
                    {
                        throw new Exception("Could not find versioned role " + versionRoleType.Name);
                    }

                    var versionedRole = @this.Strategy.GetRole(versionedRoleType.RelationType);
                    newVersion.Strategy.SetRole(versionRoleType.RelationType, versionedRole);
                }

                var allVersionsRole = versionedClass.RoleTypes.First(v => v.Name.Equals("AllVersions"));

                @this.Strategy.SetRole(currentVersionRole.RelationType, newVersion);
                @this.Strategy.AddCompositeRole(allVersionsRole.RelationType, newVersion);
            }
        }
    }
}