// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvExportFunctionTests.cs" company="Allors bvba">
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

    using NUnit.Framework;

    [TestFixture]
    public class CsvExportFunctionTests : DomainTest
    {
        [Test]
        public void Constructor()
        {
            var dutchBelgium = new Locales(this.Session).DutchBelgium;

            new C1Builder(this.Session).WithC1AllorsString("c1A").WithC1C2One2One(new C2Builder(this.Session).WithC2AllorsString("c2A").Build()).Build();
            new C1Builder(this.Session).WithC1AllorsString("c1B").WithC1C2One2One(new C2Builder(this.Session).WithC2AllorsString("c2B").Build()).Build();

            this.Session.Derive(true);

            var aclMock = new Mock<IAccessControlList>();
            aclMock.Setup(acl => acl.CanRead(It.IsAny<PropertyType>())).Returns(true);
            var acls = new AccessControlListCache(null, (allorsObject, user) => aclMock.Object);

            var export = new CsvExport("Test");
            export.Columns.Add(new CsvExportPath(M.C1.C1AllorsString));
            export.Columns.Add(new CsvExportFunction<C1>("Function", (c1, aclFactory) => CsvExport.Escape("Hello " + c1.C1AllorsString)));

            var extent = this.Session.Extent(M.C1.ObjectType).AddSort(M.C1.C1AllorsString);
            var csv = export.Write(extent, dutchBelgium, acls);

            Assert.AreEqual(
@"""C1AllorsString"";""Function""
""c1A"";""Hello c1A""
""c1B"";""Hello c1B""".Replace("\r\n", "\n"),
                    csv.Replace("\r\n", "\n"));
        }

        [Test]
        public void Locale()
        {
            var englishGreatBritain = new Locales(this.Session).EnglishGreatBritain;
            var dutchBelgium = new Locales(this.Session).DutchBelgium;

            new C1Builder(this.Session).WithC1AllorsString("c1A").WithC1AllorsDecimal(10.5M).Build();
            new C1Builder(this.Session).WithC1AllorsString("c1B").WithC1AllorsDecimal(11.5M).Build();

            this.Session.Derive(true);

            var csvFile = new CsvExport("Test");
            csvFile.Columns.Add(new CsvExportPath(M.C1.C1AllorsString));
            csvFile.Columns.Add(new CsvExportFunction<C1>(
                "Function",
                (c1, acl, cultureInfo) =>
                    {
                        if (c1.C1AllorsDecimal.HasValue)
                        {
                            return CsvExport.Escape(c1.C1AllorsDecimal.Value.ToString(cultureInfo));
                        }

                        return string.Empty;
                    }));

            var extent = this.Session.Extent(M.C1.ObjectType).AddSort(M.C1.C1AllorsString);

            var aclMock = new Mock<IAccessControlList>();
            aclMock.Setup(acl => acl.CanRead(It.IsAny<PropertyType>())).Returns(true);
            var acls = new AccessControlListCache(null, (allorsObject, user) => aclMock.Object);

            var csvEn = csvFile.Write(extent, englishGreatBritain, acls);
            var csvNl = csvFile.Write(extent, dutchBelgium, acls);

            Assert.AreNotEqual(csvEn, csvNl);

            Assert.AreEqual(
@"""C1AllorsString"";""Function""
""c1A"";""10,5""
""c1B"";""11,5""".Replace("\r\n", "\n"),
        csvNl.Replace("\r\n", "\n"));

            Assert.AreEqual(
@"""C1AllorsString"",""Function""
""c1A"",""10.5""
""c1B"",""11.5""".Replace("\r\n", "\n"),
        csvEn.Replace("\r\n", "\n"));
        }
    }
}