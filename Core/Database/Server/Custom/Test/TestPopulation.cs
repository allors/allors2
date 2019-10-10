// <copyright file="TestPopulation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Controllers
{
    using System;
    using Allors.Domain;

    public class TestPopulation
    {
        private readonly ISession session;
        private readonly string population;

        public TestPopulation(ISession session, string population)
        {
            this.session = session;
            this.population = population;
        }

        public void Apply()
        {
            if ("full".Equals(this.population))
            {
                this.Full();
            }
        }

        private void Full()
        {
            new PersonBuilder(this.session).WithUserName("noacl").WithFirstName("no").WithLastName("acl").Build();

            var noperm = new PersonBuilder(this.session).WithUserName("noperm").WithFirstName("no").WithLastName("perm").Build();
            var emptyRole = new RoleBuilder(this.session).WithName("Empty").Build();
            var acl = new AccessControlBuilder(this.session).WithRole(emptyRole).WithSubject(noperm).WithSecurityToken(this.session.GetSingleton().DefaultSecurityToken).Build();

            var c1A = new C1Builder(this.session).WithName("c1A").Build();
            var c1B = new C1Builder(this.session).WithName("c1B").Build();
            var c1C = new C1Builder(this.session).WithName("c1C").Build();
            var c1D = new C1Builder(this.session).WithName("c1D").Build();
            var c2A = new C2Builder(this.session).WithName("c2A").Build();
            var c2B = new C2Builder(this.session).WithName("c2B").Build();
            var c2C = new C2Builder(this.session).WithName("c2C").Build();
            var c2D = new C2Builder(this.session).WithName("c2D").Build();

            // class
            c1B.C1AllorsString = "ᴀbra";
            c1C.C1AllorsString = "ᴀbracadabra";
            c1D.C1AllorsString = "ᴀbracadabra";

            c2B.C2AllorsString = "ᴀbra";
            c2C.C2AllorsString = "ᴀbracadabra";
            c2D.C2AllorsString = "ᴀbracadabra";
            // exclusive interface
            c1B.I1AllorsString = "ᴀbra";
            c1C.I1AllorsString = "ᴀbracadabra";
            c1D.I1AllorsString = "ᴀbracadabra";

            // shared interface
            c1B.I12AllorsString = "ᴀbra";
            c1C.I12AllorsString = "ᴀbracadabra";
            c1D.I12AllorsString = "ᴀbracadabra";
            c2B.I12AllorsString = "ᴀbra";
            c2C.I12AllorsString = "ᴀbracadabra";
            c2D.I12AllorsString = "ᴀbracadabra";

            c1B.C1AllorsInteger = 1;
            c1C.C1AllorsInteger = 2;
            c1D.C1AllorsInteger = 2;

            c1B.I1AllorsInteger = 1;
            c1C.I1AllorsInteger = 2;
            c1D.I1AllorsInteger = 2;

            c1B.I12AllorsInteger = 1;
            c1C.I12AllorsInteger = 2;
            c1D.I12AllorsInteger = 2;
            c2B.I12AllorsInteger = 1;
            c2C.I12AllorsInteger = 2;
            c2D.I12AllorsInteger = 2;

            // DateTime
            c1B.C1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
            c1C.C1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            c1D.C1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);

            c1B.I1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
            c1C.I1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            c1D.I1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);

            c1B.I12AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
            c1C.I12AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            c1D.I12AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            c2B.I12AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
            c2C.I12AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            c2D.I12AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);

            c1B.C1AllorsDouble = 1;
            c1C.C1AllorsDouble = 2;
            c1D.C1AllorsDouble = 2;

            c1B.I1AllorsDouble = 1;
            c1C.I1AllorsDouble = 2;
            c1D.I1AllorsDouble = 2;

            c1B.I12AllorsDouble = 1;
            c1C.I12AllorsDouble = 2;
            c1D.I12AllorsDouble = 2;
            c2B.I12AllorsDouble = 1;
            c2C.I12AllorsDouble = 2;
            c2D.I12AllorsDouble = 2;

            c1B.C1AllorsDecimal = 1;
            c1C.C1AllorsDecimal = 2;
            c1D.C1AllorsDecimal = 2;

            c1B.I1AllorsDecimal = 1;
            c1C.I1AllorsDecimal = 2;
            c1D.I1AllorsDecimal = 2;

            c1B.I12AllorsDecimal = 1;
            c1C.I12AllorsDecimal = 2;
            c1D.I12AllorsDecimal = 2;
            c2B.I12AllorsDecimal = 1;
            c2C.I12AllorsDecimal = 2;
            c2D.I12AllorsDecimal = 2;

            c1B.C1C1One2One = c1B;
            c1C.C1C1One2One = c1C;
            c1D.C1C1One2One = c1D;

            c1B.C1C2One2One = c2B;
            c1C.C1C2One2One = c2C;
            c1D.C1C2One2One = c2D;

            c1B.I1I2One2One = c2B;
            c1C.I1I2One2One = c2C;
            c1D.I1I2One2One = c2D;

            c1B.I12C2One2One = c2B;
            c1C.I12C2One2One = c2C;
            c1D.I12C2One2One = c2D;
            c2A.I12C2One2One = c2A;

            c1B.C1I12One2One = c1B;
            c1C.C1I12One2One = c2B;
            c1D.C1I12One2One = c2C;

            c1B.AddC1C1One2Many(c1B);
            c1C.AddC1C1One2Many(c1C);
            c1C.AddC1C1One2Many(c1D);

            c1B.AddC1C2One2Many(c2B);
            c1C.AddC1C2One2Many(c2C);
            c1C.AddC1C2One2Many(c2D);

            c1B.AddI1I2One2Many(c2B);
            c1C.AddI1I2One2Many(c2C);
            c1C.AddI1I2One2Many(c2D);

            c1B.AddC1I12One2Many(c1B);
            c1C.AddC1I12One2Many(c2C);
            c1C.AddC1I12One2Many(c2D);

            c1B.C1C1Many2One = c1B;
            c1C.C1C1Many2One = c1C;
            c1D.C1C1Many2One = c1C;

            c1B.C1C2Many2One = c2B;
            c1C.C1C2Many2One = c2C;
            c1D.C1C2Many2One = c2C;

            c1B.I1I2Many2One = c2B;
            c1C.I1I2Many2One = c2C;
            c1D.I1I2Many2One = c2C;

            c1B.I12C2Many2One = c2B;
            c2C.I12C2Many2One = c2C;
            c2D.I12C2Many2One = c2C;

            c1B.C1I12Many2One = c1B;
            c1C.C1I12Many2One = c2C;
            c1D.C1I12Many2One = c2C;

            c1B.AddC1C1Many2Many(c1B);
            c1C.AddC1C1Many2Many(c1B);
            c1D.AddC1C1Many2Many(c1B);
            c1C.AddC1C1Many2Many(c1C);
            c1D.AddC1C1Many2Many(c1C);
            c1D.AddC1C1Many2Many(c1D);

            c1B.AddC1C2Many2Many(c2B);
            c1C.AddC1C2Many2Many(c2B);
            c1D.AddC1C2Many2Many(c2B);
            c1C.AddC1C2Many2Many(c2C);
            c1D.AddC1C2Many2Many(c2C);
            c1D.AddC1C2Many2Many(c2D);

            c1B.AddI1I2Many2Many(c2B);
            c1C.AddI1I2Many2Many(c2B);
            c1C.AddI1I2Many2Many(c2C);
            c1D.AddI1I2Many2Many(c2B);
            c1D.AddI1I2Many2Many(c2C);
            c1D.AddI1I2Many2Many(c2D);

            c1B.AddI12C2Many2Many(c2B);
            c1C.AddI12C2Many2Many(c2B);
            c1C.AddI12C2Many2Many(c2C);
            c1D.AddI12C2Many2Many(c2B);
            c1D.AddI12C2Many2Many(c2C);
            c1D.AddI12C2Many2Many(c2D);
            c2A.AddI12C2Many2Many(c2A);
            c2A.AddI12C2Many2Many(c2B);
            c2A.AddI12C2Many2Many(c2C);
            c2A.AddI12C2Many2Many(c2D);

            c1B.AddC1I12Many2Many(c1B);
            c1B.AddC1I12Many2Many(c2B);
            c1C.AddC1I12Many2Many(c2B);
            c1C.AddC1I12Many2Many(c2C);
            c1D.AddC1I12Many2Many(c2B);
            c1D.AddC1I12Many2Many(c2C);
            c1D.AddC1I12Many2Many(c2D);
        }
    }
}
