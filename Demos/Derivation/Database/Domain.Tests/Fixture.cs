// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System.Globalization;
    using Domain;

    public class Fixture
    {
        public static void Setup(IDatabase database)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB");
            CultureInfo.CurrentUICulture = new CultureInfo("en-GB");

            using (var session = database.CreateSession())
            {
                var config = new Config();
                new Setup(session, config).Apply();

                var administrator = new Users(session).GetUser("administrator");
                session.SetUser(administrator);

                var singleton = session.GetSingleton();

                new UserGroups(session).Administrators.AddMember(administrator);

                session.Derive();
                session.Commit();
            }
        }
    }
}
