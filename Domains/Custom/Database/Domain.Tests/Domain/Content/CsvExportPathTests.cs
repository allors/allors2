// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvExportPathTests.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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

namespace Domain
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Moq;

    using Xunit;

    
    public class CsvExportPathTests : DomainTest
    {
        [Fact]
        public void OperandTypeConstructor()
        {
            var dutchBelgium = new Locales(this.Session).DutchBelgium;

            new C1Builder(this.Session).WithC1AllorsString("c1A").WithC1AllorsDecimal(10.5M).Build();
            new C1Builder(this.Session).WithC1AllorsString("c1B").WithC1AllorsDecimal(11.5M).Build();

            this.Session.Derive(true);

            var csvFile = new CsvExport("Test");
            csvFile.Columns.Add(new CsvExportPath(M.C1.C1AllorsString));
            csvFile.Columns.Add(new CsvExportPath(M.C1.C1AllorsDecimal));

            var aclMock = new Mock<IAccessControlList>();
            aclMock.Setup(acl => acl.CanRead(It.IsAny<PropertyType>())).Returns(true);
            var acls = new AccessControlListCache(null, (allorsObject, user) => aclMock.Object);

            var extent = this.Session.Extent(M.C1.ObjectType).AddSort(M.C1.C1AllorsString);
            var csv = csvFile.Write(extent, dutchBelgium, acls);

            Assert.Equal(
@"""C1AllorsString"";""C1AllorsDecimal""
""c1A"";""10,5""
""c1B"";""11,5""".Replace("\r\n", "\n"),
                    csv.Replace("\r\n", "\n"));
        }

        [Fact]
        public void PathConstructor()
        {
            var dutchBelgium = new Locales(this.Session).DutchBelgium;

            new C1Builder(this.Session).WithC1AllorsString("c1A").WithC1C2One2One(new C2Builder(this.Session).WithC2AllorsString("c2A").Build()).Build();
            new C1Builder(this.Session).WithC1AllorsString("c1B").WithC1C2One2One(new C2Builder(this.Session).WithC2AllorsString("c2B").Build()).Build();

            this.Session.Derive(true);

            var csvFile = new CsvExport("Test");
            csvFile.Columns.Add(new CsvExportPath(M.C1.C1AllorsString));
            csvFile.Columns.Add(new CsvExportPath(new Path(M.C1.C1C2One2One, M.C2.C2AllorsString)));

            var extent = this.Session.Extent(M.C1.ObjectType).AddSort(M.C1.C1AllorsString);

            var aclMock = new Mock<IAccessControlList>();
            aclMock.Setup(acl => acl.CanRead(It.IsAny<PropertyType>())).Returns(true);
            var acls = new AccessControlListCache(null, (allorsObject, user) => aclMock.Object);

            var csv = csvFile.Write(extent, dutchBelgium, acls);

            Assert.Equal(
@"""C1AllorsString"";""C2AllorsString""
""c1A"";""c2A""
""c1B"";""c2B""".Replace("\r\n", "\n"),
                    csv.Replace("\r\n", "\n"));
        }

        [Fact]
        public void Locale()
        {
            this.SetIdentity("administrator");

            var englishGreatBritain = new Locales(this.Session).EnglishGreatBritain;
            var dutchBelgium = new Locales(this.Session).DutchBelgium;

            new C1Builder(this.Session).WithC1AllorsString("c1A").WithC1AllorsDecimal(10.5M).Build();
            new C1Builder(this.Session).WithC1AllorsString("c1B").WithC1AllorsDecimal(11.5M).Build();

            this.Session.Derive(true);

            var column1 = new CsvExportPath(M.C1.C1AllorsString);
            var column2 = new CsvExportPath(M.C1.C1AllorsDecimal);

            var export = new CsvExport("Test");
            export.Columns.Add(column1);
            export.Columns.Add(column2);

            var extent = this.Session.Extent(M.C1.ObjectType).AddSort(M.C1.C1AllorsString);

            var user = new Users(this.Session).GetCurrentUser();
            var acls = new AccessControlListCache(user);

            var csvEn = export.Write(extent, englishGreatBritain, acls);
            var csvNl = export.Write(extent, dutchBelgium, acls);

            Assert.NotEqual(csvEn, csvNl);
        }

        [Fact]
        public void One2Many()
        {
            var dutchBelgium = new Locales(this.Session).DutchBelgium;

            new C1Builder(this.Session)
                .WithC1AllorsString("c1A")
                .WithC1C2One2Many(new C2Builder(this.Session).WithC2AllorsString("c2A").Build())
                .Build();

            new C1Builder(this.Session)
                .WithC1AllorsString("c1B")
                .WithC1C2One2Many(new C2Builder(this.Session).WithC2AllorsString("c2B").Build())
                .WithC1C2One2Many(new C2Builder(this.Session).WithC2AllorsString("c2C").Build())
                .Build();

            this.Session.Derive(true);

            var export = new CsvExport("Test");
            export.Columns.Add(new CsvExportPath(M.C1.C1AllorsString));
            export.Columns.Add(new CsvExportPath(new Path(M.C1.C1C2One2Manies, M.C2.C2AllorsString)));

            var extent = this.Session.Extent(M.C1.ObjectType).AddSort(M.C1.C1AllorsString);

            var aclMock = new Mock<IAccessControlList>();
            aclMock.Setup(acl => acl.CanRead(It.IsAny<PropertyType>())).Returns(true);
            var acls = new AccessControlListCache(null, (allorsObject, user) => aclMock.Object);

            var csv = export.Write(extent, dutchBelgium, acls);

            Assert.Equal(
@"""C1AllorsString"";""C2AllorsString""
""c1A"";""c2A""
""c1B"";""c2B;c2C""".Replace("\r\n", "\n"),
                    csv.Replace("\r\n", "\n"));
        }
    }
}