// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestPopulation.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Database.Adapters
{
    using System;

    using Allors;

    using Allors.Domain;

    // TODO: One2One (c1->c3 & i12->c3)
    internal sealed class TestPopulation
    {
        public readonly C1 C1A;
        public readonly C1 C1B;
        public readonly C1 C1C;
        public readonly C1 C1D;
        public readonly C2 C2A;
        public readonly C2 C2B;
        public readonly C2 C2C;
        public readonly C2 C2D;
        public readonly C3 C3A;
        public readonly C3 C3B;
        public readonly C3 C3C;
        public readonly C3 C3D;
        public readonly C4 C4A;
        public readonly C4 C4B;
        public readonly C4 C4C;
        public readonly C4 C4D;

        public TestPopulation(ISession session)
        {
            this.C1A = C1.Create(session);
            this.C1B = C1.Create(session);
            this.C1C = C1.Create(session);
            this.C1D = C1.Create(session);
            this.C2A = C2.Create(session);
            this.C2B = C2.Create(session);
            this.C2C = C2.Create(session);
            this.C2D = C2.Create(session);
            this.C3A = C3.Create(session);
            this.C3B = C3.Create(session);
            this.C3C = C3.Create(session);
            this.C3D = C3.Create(session);
            this.C4A = C4.Create(session);
            this.C4B = C4.Create(session);
            this.C4C = C4.Create(session);
            this.C4D = C4.Create(session);

            // String
            // class
            this.C1B.C1AllorsString = "Abra";
            this.C1C.C1AllorsString = "Abracadabra";
            this.C1D.C1AllorsString = "Abracadabra";

            this.C1A.C1StringEquals = "Abra";
            this.C1B.C1StringEquals = "Abra";
            this.C1C.C1StringEquals = "Abra";

            this.C2B.C2AllorsString = "Abra";
            this.C2C.C2AllorsString = "Abracadabra";
            this.C2D.C2AllorsString = "Abracadabra";

            this.C3B.C3AllorsString = "Abra";
            this.C3C.C3AllorsString = "Abracadabra";
            this.C3D.C3AllorsString = "Abracadabra";

            this.C3A.C3StringEquals = "Abra";
            this.C3B.C3StringEquals = "Abra";
            this.C3C.C3StringEquals = "Abra";

            // exclusive interface
            this.C1B.I1AllorsString = "Abra";
            this.C1C.I1AllorsString = "Abracadabra";
            this.C1D.I1AllorsString = "Abracadabra";

            this.C1A.I1StringEquals = "Abra";
            this.C1B.I1StringEquals = "Abra";
            this.C1C.I1StringEquals = "Abra";

            this.C3B.I3AllorsString = "Abra";
            this.C3C.I3AllorsString = "Abracadabra";
            this.C3D.I3AllorsString = "Abracadabra";

            this.C3A.I3StringEquals = "Abra";
            this.C3B.I3StringEquals = "Abra";
            this.C3C.I3StringEquals = "Abra";

            // shared interface
            this.C1B.I12AllorsString = "Abra";
            this.C1C.I12AllorsString = "Abracadabra";
            this.C1D.I12AllorsString = "Abracadabra";
            this.C2B.I12AllorsString = "Abra";
            this.C2C.I12AllorsString = "Abracadabra";
            this.C2D.I12AllorsString = "Abracadabra";

            this.C2B.I23AllorsString = "Abra";
            this.C2C.I23AllorsString = "Abracadabra";
            this.C2D.I23AllorsString = "Abracadabra";
            this.C3B.I23AllorsString = "Abra";
            this.C3C.I23AllorsString = "Abracadabra";
            this.C3D.I23AllorsString = "Abracadabra";

            this.C3B.I34AllorsString = "Abra";
            this.C3C.I34AllorsString = "Abracadabra";
            this.C3D.I34AllorsString = "Abracadabra";
            this.C4B.I34AllorsString = "Abra";
            this.C4C.I34AllorsString = "Abracadabra";
            this.C4D.I34AllorsString = "Abracadabra";

            this.C1B.S1AllorsString = "Abra";
            this.C1C.S1AllorsString = "Abracadabra";
            this.C1D.S1AllorsString = "Abracadabra";

            this.C1B.S1234AllorsString = "Abra";
            this.C1C.S1234AllorsString = "Abracadabra";
            this.C1D.S1234AllorsString = "Abracadabra";
            this.C2B.S1234AllorsString = "Abra";
            this.C2C.S1234AllorsString = "Abracadabra";
            this.C2D.S1234AllorsString = "Abracadabra";
            this.C3B.S1234AllorsString = "Abra";
            this.C3C.S1234AllorsString = "Abracadabra";
            this.C3D.S1234AllorsString = "Abracadabra";
            this.C4B.S1234AllorsString = "Abra";
            this.C4C.S1234AllorsString = "Abracadabra";
            this.C4D.S1234AllorsString = "Abracadabra";

            // Integer
            this.C1B.C1AllorsInteger = 1;
            this.C1C.C1AllorsInteger = 2;
            this.C1D.C1AllorsInteger = 2;

            this.C1B.C1IntegerLessThan = 0;
            this.C1C.C1IntegerLessThan = 2;
            this.C1D.C1IntegerLessThan = 4;

            this.C1B.C1IntegerGreaterThan = 0;
            this.C1C.C1IntegerGreaterThan = 2;
            this.C1D.C1IntegerGreaterThan = 4;

            this.C1B.C1IntegerBetweenA = -10;
            this.C1B.C1IntegerBetweenB = 0;
            this.C1C.C1IntegerBetweenA = 2;
            this.C1C.C1IntegerBetweenB = 2;
            this.C1D.C1IntegerBetweenA = 0;
            this.C1D.C1IntegerBetweenB = 10;

            this.C1B.I1AllorsInteger = 1;
            this.C1C.I1AllorsInteger = 2;
            this.C1D.I1AllorsInteger = 2;

            this.C1B.S1AllorsInteger = 1;
            this.C1C.S1AllorsInteger = 2;
            this.C1D.S1AllorsInteger = 2;

            this.C1B.I12AllorsInteger = 1;
            this.C1C.I12AllorsInteger = 2;
            this.C1D.I12AllorsInteger = 2;
            this.C2B.I12AllorsInteger = 1;
            this.C2C.I12AllorsInteger = 2;
            this.C2D.I12AllorsInteger = 2;

            this.C1B.S1234AllorsInteger = 1;
            this.C1C.S1234AllorsInteger = 2;
            this.C1D.S1234AllorsInteger = 2;
            this.C2B.S1234AllorsInteger = 1;
            this.C2C.S1234AllorsInteger = 2;
            this.C2D.S1234AllorsInteger = 2;
            this.C3B.S1234AllorsInteger = 1;
            this.C3C.S1234AllorsInteger = 2;
            this.C3D.S1234AllorsInteger = 2;
            this.C4B.S1234AllorsInteger = 1;
            this.C4C.S1234AllorsInteger = 2;
            this.C4D.S1234AllorsInteger = 2;

            // DateTime
            this.C1B.C1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
            this.C1C.C1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C1D.C1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);

            this.C1B.C1DateTimeLessThan = new DateTime(2000, 1, 1, 0, 0, 3, DateTimeKind.Utc);
            this.C1C.C1DateTimeLessThan = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C1D.C1DateTimeLessThan = new DateTime(2000, 1, 1, 0, 0, 7, DateTimeKind.Utc);

            this.C1B.C1DateTimeGreaterThan = new DateTime(2000, 1, 1, 0, 0, 3, DateTimeKind.Utc);
            this.C1C.C1DateTimeGreaterThan = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C1D.C1DateTimeGreaterThan = new DateTime(2000, 1, 1, 0, 0, 7, DateTimeKind.Utc);

            this.C1B.C1DateTimeBetweenA = new DateTime(2000, 1, 1, 0, 0, 1, DateTimeKind.Utc);
            this.C1B.C1DateTimeBetweenB = new DateTime(2000, 1, 1, 0, 0, 3, DateTimeKind.Utc);
            this.C1C.C1DateTimeBetweenA = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C1C.C1DateTimeBetweenB = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C1D.C1DateTimeBetweenA = new DateTime(2000, 1, 1, 0, 0, 3, DateTimeKind.Utc);
            this.C1D.C1DateTimeBetweenB = new DateTime(2000, 1, 1, 0, 0, 10, DateTimeKind.Utc);

            this.C1B.I1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
            this.C1C.I1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C1D.I1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);

            this.C1B.S1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
            this.C1C.S1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C1D.S1AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);

            this.C1B.I12AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
            this.C1C.I12AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C1D.I12AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C2B.I12AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
            this.C2C.I12AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C2D.I12AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);

            this.C1B.S1234AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
            this.C1C.S1234AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C1D.S1234AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C2B.S1234AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
            this.C2C.S1234AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C2D.S1234AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C3B.S1234AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
            this.C3C.S1234AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C3D.S1234AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C4B.S1234AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
            this.C4C.S1234AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            this.C4D.S1234AllorsDateTime = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
            
            // Float
            this.C1B.C1AllorsDouble = 1;
            this.C1C.C1AllorsDouble = 2;
            this.C1D.C1AllorsDouble = 2;

            this.C1B.C1FloatLessThan = 0;
            this.C1C.C1FloatLessThan = 2;
            this.C1D.C1FloatLessThan = 4;

            this.C1B.C1FloatGreaterThan = 0;
            this.C1C.C1FloatGreaterThan = 2;
            this.C1D.C1FloatGreaterThan = 4;

            this.C1B.C1FloatBetweenA = -10;
            this.C1B.C1FloatBetweenB = 0;
            this.C1C.C1FloatBetweenA = 2;
            this.C1C.C1FloatBetweenB = 2;
            this.C1D.C1FloatBetweenA = 0;
            this.C1D.C1FloatBetweenB = 10;

            this.C1B.I1AllorsDouble = 1;
            this.C1C.I1AllorsDouble = 2;
            this.C1D.I1AllorsDouble = 2;

            this.C1B.S1AllorsDouble = 1;
            this.C1C.S1AllorsDouble = 2;
            this.C1D.S1AllorsDouble = 2;

            this.C1B.I12AllorsDouble = 1;
            this.C1C.I12AllorsDouble = 2;
            this.C1D.I12AllorsDouble = 2;
            this.C2B.I12AllorsDouble = 1;
            this.C2C.I12AllorsDouble = 2;
            this.C2D.I12AllorsDouble = 2;

            this.C1B.S1234AllorsDouble = 1;
            this.C1C.S1234AllorsDouble = 2;
            this.C1D.S1234AllorsDouble = 2;
            this.C2B.S1234AllorsDouble = 1;
            this.C2C.S1234AllorsDouble = 2;
            this.C2D.S1234AllorsDouble = 2;
            this.C3B.S1234AllorsDouble = 1;
            this.C3C.S1234AllorsDouble = 2;
            this.C3D.S1234AllorsDouble = 2;
            this.C4B.S1234AllorsDouble = 1;
            this.C4C.S1234AllorsDouble = 2;
            this.C4D.S1234AllorsDouble = 2;

            // Decimal
            this.C1B.C1AllorsDecimal = 1;
            this.C1C.C1AllorsDecimal = 2;
            this.C1D.C1AllorsDecimal = 2;

            this.C1B.C1DecimalLessThan = 0;
            this.C1C.C1DecimalLessThan = 2;
            this.C1D.C1DecimalLessThan = 4;

            this.C1B.C1DecimalGreaterThan = 0;
            this.C1C.C1DecimalGreaterThan = 2;
            this.C1D.C1DecimalGreaterThan = 4;

            this.C1B.C1DecimalBetweenA = -10;
            this.C1B.C1DecimalBetweenB = 0;
            this.C1C.C1DecimalBetweenA = 2;
            this.C1C.C1DecimalBetweenB = 2;
            this.C1D.C1DecimalBetweenA = 0;
            this.C1D.C1DecimalBetweenB = 10;

            this.C1B.I1AllorsDecimal = 1;
            this.C1C.I1AllorsDecimal = 2;
            this.C1D.I1AllorsDecimal = 2;

            this.C1B.S1AllorsDecimal = 1;
            this.C1C.S1AllorsDecimal = 2;
            this.C1D.S1AllorsDecimal = 2;

            this.C1B.I12AllorsDecimal = 1;
            this.C1C.I12AllorsDecimal = 2;
            this.C1D.I12AllorsDecimal = 2;
            this.C2B.I12AllorsDecimal = 1;
            this.C2C.I12AllorsDecimal = 2;
            this.C2D.I12AllorsDecimal = 2;

            this.C1B.S1234AllorsDecimal = 1;
            this.C1C.S1234AllorsDecimal = 2;
            this.C1D.S1234AllorsDecimal = 2;
            this.C2B.S1234AllorsDecimal = 1;
            this.C2C.S1234AllorsDecimal = 2;
            this.C2D.S1234AllorsDecimal = 2;
            this.C3B.S1234AllorsDecimal = 1;
            this.C3C.S1234AllorsDecimal = 2;
            this.C3D.S1234AllorsDecimal = 2;
            this.C4B.S1234AllorsDecimal = 1;
            this.C4C.S1234AllorsDecimal = 2;
            this.C4D.S1234AllorsDecimal = 2;

            // Composites
            this.C1B.C1C1one2one = this.C1B;
            this.C1C.C1C1one2one = this.C1C;
            this.C1D.C1C1one2one = this.C1D;

            this.C1B.C1C2one2one = this.C2B;
            this.C1C.C1C2one2one = this.C2C;
            this.C1D.C1C2one2one = this.C2D;

            this.C1B.C1C3one2one = this.C3B;
            this.C1C.C1C3one2one = this.C3C;
            this.C1D.C1C3one2one = this.C3D;

            this.C3B.C3C4one2one = this.C4B;
            this.C3C.C3C4one2one = this.C4C;
            this.C3D.C3C4one2one = this.C4D;

            this.C1B.I1I2one2one = this.C2B;
            this.C1C.I1I2one2one = this.C2C;
            this.C1D.I1I2one2one = this.C2D;

            this.C1B.S1S2one2one = this.C2B;
            this.C1C.S1S2one2one = this.C2C;
            this.C1D.S1S2one2one = this.C2D;

            this.C1B.I12C2one2one = this.C2B;
            this.C1C.I12C2one2one = this.C2C;
            this.C1D.I12C2one2one = this.C2D;
            this.C2A.I12C2one2one = this.C2A;

            this.C1B.I12C3one2one = this.C3B;
            this.C1C.I12C3one2one = this.C3C;
            this.C1D.I12C3one2one = this.C3D;
            this.C2A.I12C3one2one = this.C3A;

            this.C1B.S1234C2one2one = this.C2B;
            this.C1C.S1234C2one2one = this.C2C;
            this.C1D.S1234C2one2one = this.C2D;
            this.C2A.S1234C2one2one = this.C2A;

            this.C1B.C1I12one2one = this.C1B;
            this.C1C.C1I12one2one = this.C2B;
            this.C1D.C1I12one2one = this.C2C;

            this.C1B.S1234one2one = this.C1B;
            this.C1C.S1234one2one = this.C2B;
            this.C1D.S1234one2one = this.C3B;
            this.C2B.S1234one2one = this.C1C;
            this.C2C.S1234one2one = this.C2C;
            this.C2D.S1234one2one = this.C3C;
            this.C3B.S1234one2one = this.C1D;
            this.C3C.S1234one2one = this.C2D;
            this.C3D.S1234one2one = this.C3D;

            this.C1B.AddC1C1one2many(this.C1B);
            this.C1C.AddC1C1one2many(this.C1C);
            this.C1C.AddC1C1one2many(this.C1D);

            this.C1B.AddC1C2one2many(this.C2B);
            this.C1C.AddC1C2one2many(this.C2C);
            this.C1C.AddC1C2one2many(this.C2D);

            this.C3B.AddC3C4one2many(this.C4B);
            this.C3C.AddC3C4one2many(this.C4C);
            this.C3C.AddC3C4one2many(this.C4D);

            this.C1B.AddI1I2one2many(this.C2B);
            this.C1C.AddI1I2one2many(this.C2C);
            this.C1C.AddI1I2one2many(this.C2D);

            this.C1B.AddS1S2one2many(this.C2B);
            this.C1C.AddS1S2one2many(this.C2C);
            this.C1C.AddS1S2one2many(this.C2D);

            this.C1B.AddI12C2one2many(this.C2B);
            this.C2C.AddI12C2one2many(this.C2C);
            this.C2C.AddI12C2one2many(this.C2D);

            this.C1B.AddS1234C2one2many(this.C2B);
            this.C3C.AddS1234C2one2many(this.C2C);
            this.C3C.AddS1234C2one2many(this.C2D);

            this.C1B.AddC1I12one2many(this.C1B);
            this.C1C.AddC1I12one2many(this.C2C);
            this.C1C.AddC1I12one2many(this.C2D);

            this.C1B.AddS1234one2many(this.C1B);
            this.C3C.AddS1234one2many(this.C1C);
            this.C3C.AddS1234one2many(this.C1D);

            this.C1B.C1C1many2one = this.C1B;
            this.C1C.C1C1many2one = this.C1C;
            this.C1D.C1C1many2one = this.C1C;

            this.C1B.C1C2many2one = this.C2B;
            this.C1C.C1C2many2one = this.C2C;
            this.C1D.C1C2many2one = this.C2C;

            this.C3B.C3C4many2one = this.C4B;
            this.C3C.C3C4many2one = this.C4C;
            this.C3D.C3C4many2one = this.C4C;

            this.C1B.I1I2many2one = this.C2B;
            this.C1C.I1I2many2one = this.C2C;
            this.C1D.I1I2many2one = this.C2C;

            this.C1B.S1S2many2one = this.C2B;
            this.C1C.S1S2many2one = this.C2C;
            this.C1D.S1S2many2one = this.C2C;

            this.C1B.I12C2many2one = this.C2B;
            this.C2C.I12C2many2one = this.C2C;
            this.C2D.I12C2many2one = this.C2C;

            this.C1B.S1234C2many2one = this.C2B;
            this.C3C.S1234C2many2one = this.C2C;
            this.C3D.S1234C2many2one = this.C2C;

            this.C1B.C1I12many2one = this.C1B;
            this.C1C.C1I12many2one = this.C2C;
            this.C1D.C1I12many2one = this.C2C;

            this.C1B.S1234many2one = this.C1B;
            this.C3C.S1234many2one = this.C1C;
            this.C3D.S1234many2one = this.C1C;

            this.C1B.AddC1C1many2many(this.C1B);
            this.C1C.AddC1C1many2many(this.C1B);
            this.C1D.AddC1C1many2many(this.C1B);
            this.C1C.AddC1C1many2many(this.C1C);
            this.C1D.AddC1C1many2many(this.C1C);
            this.C1D.AddC1C1many2many(this.C1D);

            this.C1B.AddC1C2many2many(this.C2B);
            this.C1C.AddC1C2many2many(this.C2B);
            this.C1D.AddC1C2many2many(this.C2B);
            this.C1C.AddC1C2many2many(this.C2C);
            this.C1D.AddC1C2many2many(this.C2C);
            this.C1D.AddC1C2many2many(this.C2D);

            this.C1B.AddI1I2many2many(this.C2B);
            this.C1C.AddI1I2many2many(this.C2B);
            this.C1C.AddI1I2many2many(this.C2C);
            this.C1D.AddI1I2many2many(this.C2B);
            this.C1D.AddI1I2many2many(this.C2C);
            this.C1D.AddI1I2many2many(this.C2D);

            this.C1B.AddS1S2many2many(this.C2B);
            this.C1C.AddS1S2many2many(this.C2B);
            this.C1C.AddS1S2many2many(this.C2C);
            this.C1D.AddS1S2many2many(this.C2B);
            this.C1D.AddS1S2many2many(this.C2C);
            this.C1D.AddS1S2many2many(this.C2D);

            this.C1B.AddI12C2many2many(this.C2B);
            this.C1C.AddI12C2many2many(this.C2B);
            this.C1C.AddI12C2many2many(this.C2C);
            this.C1D.AddI12C2many2many(this.C2B);
            this.C1D.AddI12C2many2many(this.C2C);
            this.C1D.AddI12C2many2many(this.C2D);
            this.C2A.AddI12C2many2many(this.C2A);
            this.C2A.AddI12C2many2many(this.C2B);
            this.C2A.AddI12C2many2many(this.C2C);
            this.C2A.AddI12C2many2many(this.C2D);

            this.C1B.AddS1234C2many2many(this.C2B);
            this.C1C.AddS1234C2many2many(this.C2B);
            this.C1C.AddS1234C2many2many(this.C2C);
            this.C1D.AddS1234C2many2many(this.C2B);
            this.C1D.AddS1234C2many2many(this.C2C);
            this.C1D.AddS1234C2many2many(this.C2D);
            this.C2A.AddS1234C2many2many(this.C2A);
            this.C2A.AddS1234C2many2many(this.C2B);
            this.C2A.AddS1234C2many2many(this.C2C);
            this.C2A.AddS1234C2many2many(this.C2D);

            this.C1B.AddC1I12many2many(this.C1B);
            this.C1B.AddC1I12many2many(this.C2B);
            this.C1C.AddC1I12many2many(this.C2B);
            this.C1C.AddC1I12many2many(this.C2C);
            this.C1D.AddC1I12many2many(this.C2B);
            this.C1D.AddC1I12many2many(this.C2C);
            this.C1D.AddC1I12many2many(this.C2D);

            this.C1B.AddS1234many2many(this.C1B);
            this.C1B.AddS1234many2many(this.C1A);
            this.C1C.AddS1234many2many(this.C2B);
            this.C1C.AddS1234many2many(this.C1A);
            this.C1D.AddS1234many2many(this.C3B);
            this.C1D.AddS1234many2many(this.C1A);
            this.C2B.AddS1234many2many(this.C1C);
            this.C2B.AddS1234many2many(this.C1A);
            this.C2C.AddS1234many2many(this.C2C);
            this.C2C.AddS1234many2many(this.C1A);
            this.C2D.AddS1234many2many(this.C3C);
            this.C2D.AddS1234many2many(this.C1A);
            this.C3B.AddS1234many2many(this.C1D);
            this.C3B.AddS1234many2many(this.C1A);
            this.C3C.AddS1234many2many(this.C2D);
            this.C3C.AddS1234many2many(this.C1A);
            this.C3D.AddS1234many2many(this.C3D);
            this.C3D.AddS1234many2many(this.C1A);

            this.C1B.ClassName = "c1";
            this.C3B.ClassName = "c3";
        }
    }
}