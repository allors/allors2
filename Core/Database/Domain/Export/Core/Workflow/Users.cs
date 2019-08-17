// <copyright file="Users.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    using Allors;

    /// <summary>
    /// A user of this application.
    /// </summary>
    public partial class Users
    {
        public const string GuestUserName = "Guest";
        public const string AdministratorUserName = "Administrator";

        public User GetUser(string userId)
        {
            // TODO: cache
            var user = this.FindBy(this.Meta.UserName, userId);
            return user;
        }

        public void SavePasswords(XmlWriter writer)
        {
            var usersWithPassword = this.Extent();
            usersWithPassword.Filter.AddExists(this.Meta.UserPasswordHash);

            var records = new List<Credentials.Record>();
            foreach (User user in usersWithPassword)
            {
                records.Add(new Credentials.Record
                {
                    UserName = user.UserName,
                    PasswordHash = user.UserPasswordHash,
                });
            }

            var credentials = new Credentials { Records = records.ToArray() };
            var xmlSerializer = new XmlSerializer(typeof(Credentials));
            xmlSerializer.Serialize(writer, credentials);
        }

        public void LoadPasswords(XmlReader reader)
        {
            var xmlSerializer = new XmlSerializer(typeof(Credentials));
            var credentials = (Credentials)xmlSerializer.Deserialize(reader);
            foreach (var credential in credentials.Records)
            {
                var user = this.FindBy(this.Meta.UserName, credential.UserName);
                if (user != null)
                {
                    user.UserPasswordHash = credential.PasswordHash;
                }
            }
        }

        [XmlRoot("Credentials")]
        public class Credentials
        {
            [XmlElement("Credential", typeof(Record))]
            public Record[] Records { get; set; }

            public class Record
            {
                public string UserName { get; set; }

                public string PasswordHash { get; set; }
            }
        }

        private class CachedUser
        {
            private readonly string objectId;

            public CachedUser(User user)
            {
                this.objectId = user.Id.ToString();
                this.UserId = user.UserName.ToLower();
            }

            public string UserId { get; set; }

            public User GetUser(ISession session) => (User)session.Instantiate(this.objectId);
        }
    }
}
