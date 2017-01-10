// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllorsRoleProvider.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Web.Security
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Security;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    public class AllorsRoleProvider : RoleProvider
    {
        private static readonly string[] EmptyStringArray = new string[0];

        public override string ApplicationName { get; set; }

        #region Not Supported

        public override void CreateRole(string roleName)
        {
            throw new NotSupportedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotSupportedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        #endregion Not Supported

        public override bool IsUserInRole(string username, string roleName)
        {
            return Array.IndexOf(this.GetRolesForUser(username), roleName) >= 0;
        }

        public override string[] GetRolesForUser(string username)
        {
            var roles = new List<string>();
            if (!string.IsNullOrWhiteSpace(username))
            {
                var database = Config.Default;

                using (ISession session = database.CreateSession())
                {
                    var user = new Users(session).FindBy(M.User.UserName, username);
                    if (user != null)
                    {
                        foreach (UserGroup userGroup in user.UserGroupsWhereMember)
                        {
                            roles.Add(userGroup.Name);
                        }
                    }
                }
            }
            
            return roles.ToArray();
        }

        public override bool RoleExists(string roleName)
        {
            return Array.IndexOf(this.GetAllRoles(), roleName) >= 0;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            var database = Config.Default;

            var userIds = new ArrayList();
            using (ISession session = database.CreateSession())
            {
                var userGroup = new UserGroups(session).FindBy(M.UserGroup.Name, roleName);
                if (userGroup != null)
                {
                    foreach (User user in userGroup.Members)
                    {
                        userIds.Add(user.UserName);
                    }
                }
            }

            return (string[])userIds.ToArray(typeof(string));
        }

        public override string[] GetAllRoles()
        {
            var database = Config.Default;

            var roleNames = new ArrayList();
            using (ISession session = database.CreateSession())
            {
                foreach (UserGroup group in new UserGroups(session).Extent())
                {
                    roleNames.Add(group.Name);
                }
            }

            return (string[])roleNames.ToArray(typeof(string));
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            foreach (var userName in this.GetUsersInRole(roleName))
            {
                if (usernameToMatch.Equals(userName))
                {
                    return new[] { userName };
                }
            }

            return EmptyStringArray;
        }
    }
}