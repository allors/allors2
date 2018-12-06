// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationTest.cs" company="Allors bvba">
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

namespace Allors.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    public abstract class SerializationTest : IDisposable
    {
        protected static readonly bool[] TrueFalse = { true, false };
        private static readonly string GuidString = Guid.NewGuid().ToString();

        #region population
        private C1 c1A;
        private C1 c1B;
        private C1 c1C;
        private C1 c1D;
        private C1 c1Empty;
        private C2 c2A;
        private C2 c2B;
        private C2 c2C;
        private C2 c2D;
        private C3 c3A;
        private C3 c3B;
        private C3 c3C;
        private C3 c3D;
        private C4 c4A;
        private C4 c4B;
        private C4 c4C;
        private C4 c4D;
        #endregion

        protected virtual bool EmptyStringIsNull => false;

        protected abstract IProfile Profile { get; }

        protected IDatabase Population => this.Profile.Database;

        protected ISession Session => this.Profile.Session;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

        public abstract void Dispose();

        [Fact]
        public void DifferentVersion()
        {
            foreach (var init in this.Inits)
            {
                init();

                var otherPopulation = this.CreatePopulation();
                using (var otherSession = otherPopulation.CreateSession())
                {
                    this.Populate(otherSession);
                    otherSession.Commit();

                    var stringWriter = new StringWriter();
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        otherPopulation.Save(writer);
                    }

                    var xml = stringWriter.ToString();
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(xml);
                    var populationElement = (XmlElement)xmlDocument.SelectSingleNode("//population");
                    populationElement.SetAttribute("version", "0");
                    xml = xmlDocument.OuterXml;

                    try
                    {
                        using (var stringReader = new StringReader(xml))
                        {
                            using (var reader = XmlReader.Create(stringReader))
                            {
                                this.Population.Load(reader);
                            }
                        }

                        Assert.True(false); // Fail
                    }
                    catch (ArgumentException)
                    {
                    }

                    populationElement.SetAttribute("version", "2");
                    xml = xmlDocument.OuterXml;

                    try
                    {
                        using (var stringReader = new StringReader(xml))
                        {
                            using (var reader = XmlReader.Create(stringReader))
                            {
                                this.Population.Load(reader);
                            }
                        }

                        Assert.True(false); // Fail
                    }
                    catch (ArgumentException)
                    {
                    }

                    populationElement.SetAttribute("version", "a");
                    xml = xmlDocument.OuterXml;

                    var exception = false;
                    try
                    {
                        using (var stringReader = new StringReader(xml))
                        {
                            using (var reader = XmlReader.Create(stringReader))
                            {
                                this.Population.Load(reader);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        exception = true;
                    }

                    Assert.True(exception);

                    populationElement.SetAttribute("version", string.Empty);
                    xml = xmlDocument.OuterXml;

                    var exceptionThrown = false;
                    try
                    {
                        using (var stringReader = new StringReader(xml))
                        {
                            using (var reader = XmlReader.Create(stringReader))
                            {
                                this.Population.Load(reader);
                            }
                        }
                    }
                    catch (ArgumentException)
                    {
                        exceptionThrown = true;
                    }
                    catch (InvalidOperationException)
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);
                }
            }
        }

        [Fact]
        public void Load()
        {
            foreach (var indentation in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var otherPopulation = this.CreatePopulation();
                    using (var otherSession = otherPopulation.CreateSession())
                    {
                        this.Populate(otherSession);
                        otherSession.Commit();

                        var stringWriter = new StringWriter();
                        var xmlWriterSettings = new XmlWriterSettings { Indent = indentation };
                        using (var writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
                        {
                            otherPopulation.Save(writer);
                        }

                        var xml = stringWriter.ToString();
                        File.WriteAllText(@"c:\temp\population.xml", xml);
                        //Console.Out.WriteLine(xml);

                        var stringReader = new StringReader(xml);
                        using (var reader = XmlReader.Create(stringReader))
                        {
                            this.Population.Load(reader);
                        }


                        using (var session = this.Population.CreateSession())
                        {
                            var x = (C1)session.Instantiate(1);
                            var str = x.C1AllorsString;

                            this.AssertPopulation(session);
                        }
                    }
                }
            }
        }

        [Fact]
        public void LoadVersions()
        {
            foreach (var init in this.Inits)
            {
                init();

                var otherPopulation = this.CreatePopulation();
                using (var otherSession = otherPopulation.CreateSession())
                {
                    // Initial
                    var otherC1 = otherSession.Create<C1>();

                    otherSession.Commit();

                    var initialObjectVersion = otherC1.Strategy.ObjectVersion;

                    var xml = DoSave(otherPopulation);
                    DoLoad(this.Population, xml);

                    using (var session = this.Population.CreateSession())
                    {
                        var c1 = session.Instantiate(otherC1.Id);

                        Assert.Equal(otherC1.Strategy.ObjectVersion, c1.Strategy.ObjectVersion);
                    }

                    // Change
                    otherC1.C1AllorsString = "Changed";

                    otherSession.Commit();

                    var changedObjectVersion = otherC1.Strategy.ObjectVersion;

                    xml = DoSave(otherPopulation);
                    DoLoad(this.Population, xml);

                    using (var session = this.Population.CreateSession())
                    {
                        var c1 = session.Instantiate(otherC1.Id);

                        Assert.Equal(otherC1.Strategy.ObjectVersion, c1.Strategy.ObjectVersion);
                        Assert.NotEqual(initialObjectVersion, c1.Strategy.ObjectVersion);
                    }

                    // Change again
                    otherC1.C1AllorsString = "Changed again";

                    otherSession.Commit();

                    xml = DoSave(otherPopulation);
                    DoLoad(this.Population, xml);

                    using (var session = this.Population.CreateSession())
                    {
                        var c1 = session.Instantiate(otherC1.Id);

                        Assert.Equal(otherC1.Strategy.ObjectVersion, c1.Strategy.ObjectVersion);
                        Assert.NotEqual(initialObjectVersion, c1.Strategy.ObjectVersion);
                        Assert.NotEqual(changedObjectVersion, c1.Strategy.ObjectVersion);
                    }
                }
            }
        }

        [Fact]
        public void LoadRollback()
        {
            foreach (var init in this.Inits)
            {
                init();

                var otherPopulation = this.CreatePopulation();
                using (var otherSession = otherPopulation.CreateSession())
                {
                    this.Populate(otherSession);
                    otherSession.Commit();

                    var stringWriter = new StringWriter();
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        otherPopulation.Save(writer);
                    }

                    var xml = stringWriter.ToString();
                    Console.Out.WriteLine(xml);

                    var stringReader = new StringReader(xml);
                    using (var reader = XmlReader.Create(stringReader))
                    {
                        this.Population.Load(reader);
                    }

                    using (var session = this.Population.CreateSession())
                    {
                        session.Rollback();

                        this.AssertPopulation(session);
                    }
                }
            }
        }

        [Fact]
        public void LoadDifferenMode()
        {
            foreach (var init in this.Inits)
            {
                init();

                var population = this.CreatePopulation();
                var session = population.CreateSession();

                try
                {
                    this.Populate(session);
                    session.Commit();

                    var stringWriter = new StringWriter();
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        population.Save(writer);
                    }

                    Dump(population);

                    var stringReader = new StringReader(stringWriter.ToString());
                    var reader = XmlReader.Create(stringReader);

                    try
                    {
                        this.Population.Load(reader);
                        Assert.True(false); // Fail
                    }
                    catch
                    {
                    }
                }
                finally
                {
                    session.Commit();
                }
            }
        }

        [Fact]
        public void LoadDifferentCultureInfos()
        {
            foreach (var init in this.Inits)
            {
                init();

                var writeCultureInfo = new CultureInfo("en-US");
                var readCultureInfo = new CultureInfo("en-GB");

                CultureInfo.CurrentCulture = writeCultureInfo;
                CultureInfo.CurrentUICulture = writeCultureInfo;

                var loadPopulation = this.CreatePopulation();
                var loadSession = loadPopulation.CreateSession();
                this.Populate(loadSession);

                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    loadSession.Database.Save(writer);
                }

                CultureInfo.CurrentCulture = readCultureInfo;
                CultureInfo.CurrentUICulture = readCultureInfo;

                var xml = stringWriter.ToString();
                var stringReader = new StringReader(xml);
                using (var reader = XmlReader.Create(stringReader))
                {
                    this.Population.Load(reader);
                }

                using (var session = this.Population.CreateSession())
                {
                    this.AssertPopulation(session);
                }

                loadSession.Rollback();
            }
        }

        [Fact]
        public void LoadDifferenVersion()
        {
            foreach (var init in this.Inits)
            {
                init();


                var population = this.CreatePopulation();
                var session = population.CreateSession();

                try
                {
                    this.Populate(session);
                    session.Commit();

                    var stringWriter = new StringWriter();
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        population.Save(writer);
                    }

                    Dump(population);

                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(stringWriter.ToString());
                    var allorsElement = (XmlElement)xmlDocument.SelectSingleNode("/allors");
                    allorsElement.SetAttribute("version", "0.9");

                    var stringReader = new StringReader(xmlDocument.InnerText);
                    var reader = XmlReader.Create(stringReader);

                    try
                    {
                        this.Population.Load(reader);
                        Assert.True(false); // Fail
                    }
                    catch
                    {
                    }
                }
                finally
                {
                    session.Commit();
                }
            }
        }

        [Fact]
        public void LoadSpecial()
        {
            foreach (var init in this.Inits)
            {
                init();

                var savePopulation = this.CreatePopulation();
                var saveSession = savePopulation.CreateSession();

                try
                {
                    this.c1A = C1.Create(saveSession);
                    this.c1A.C1AllorsString = "> <";
                    this.c1A.I12AllorsString = "< >";
                    this.c1A.I1AllorsString = "& &&";
                    this.c1A.S1AllorsString = "' \" ''";

                    this.c1Empty = C1.Create(saveSession);

                    saveSession.Commit();

                    var stringWriter = new StringWriter();
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        savePopulation.Save(writer);
                    }

                    //writer = XmlWriter.Create(@"population.xml", Encoding.UTF8);
                    //saveSession.Population.Save(writer);
                    //writer.Close();

                    var stringReader = new StringReader(stringWriter.ToString());
                    using (var reader = XmlReader.Create(stringReader))
                    {
                        this.Population.Load(reader);
                    }

                    using (var session = this.Population.CreateSession())
                    {
                        var copyValues = C1.Instantiate(session, this.c1A.Strategy.ObjectId);

                        Assert.Equal(this.c1A.C1AllorsString, copyValues.C1AllorsString);
                        Assert.Equal(this.c1A.I12AllorsString, copyValues.I12AllorsString);
                        Assert.Equal(this.c1A.I1AllorsString, copyValues.I1AllorsString);
                        Assert.Equal(this.c1A.S1AllorsString, copyValues.S1AllorsString);

                        var c1EmptyLoaded = C1.Instantiate(session, this.c1Empty.Strategy.ObjectId);
                        Assert.NotNull(c1EmptyLoaded);
                    }
                }
                finally
                {
                    saveSession.Rollback();
                }
            }
        }

        [Fact]
        public void Save()
        {
            foreach (var init in this.Inits)
            {
                init();

                using (var session = this.Population.CreateSession())
                {
                    this.Populate(session);

                    var stringWriter = new StringWriter();
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        this.Population.Save(writer);
                    }

                    ////writer = XmlWriter.Create("population.xml", new UTF8Encoding());
                    ////this.Population.Save(writer);
                    ////writer.Close();

                    var xml = stringWriter.ToString();

                    var stringReader = new StringReader(xml);
                    using (var reader = XmlReader.Create(stringReader))
                    {
                        var savePopulation = this.CreatePopulation();
                        savePopulation.Load(reader);

                        using (var saveSession = savePopulation.CreateSession())
                        {
                            this.AssertPopulation(saveSession);
                        }
                    }
                }
            }
        }

        [Fact]
        public void SaveVersions()
        {
            foreach (var init in this.Inits)
            {
                init();

                using (var session = this.Population.CreateSession())
                {
                    // Initial
                    var c1 = session.Create<C1>();

                    session.Commit();

                    var initialObjectVersion = c1.Strategy.ObjectVersion;

                    var xml = DoSave(this.Population);

                    var otherPopulation = this.CreatePopulation();
                    DoLoad(otherPopulation, xml);

                    using (var otherSession = otherPopulation.CreateSession())
                    {
                        var otherC1 = otherSession.Instantiate(c1.Id);

                        Assert.Equal(c1.Strategy.ObjectVersion, otherC1.Strategy.ObjectVersion);
                    }


                    // Change
                    c1.C1AllorsString = "Changed";

                    session.Commit();

                    var changedObjectVersion = c1.Strategy.ObjectVersion;

                    xml = DoSave(this.Population);

                    otherPopulation = this.CreatePopulation();
                    DoLoad(otherPopulation, xml);

                    using (var otherSession = otherPopulation.CreateSession())
                    {
                        var otherC1 = otherSession.Instantiate(c1.Id);

                        Assert.Equal(c1.Strategy.ObjectVersion, otherC1.Strategy.ObjectVersion);
                        Assert.NotEqual(initialObjectVersion, otherC1.Strategy.ObjectVersion);
                    }

                    // Change again
                    c1.C1AllorsString = "Changed again";

                    session.Commit();

                    xml = DoSave(this.Population);

                    otherPopulation = this.CreatePopulation();
                    DoLoad(otherPopulation, xml);

                    using (var otherSession = otherPopulation.CreateSession())
                    {
                        var otherC1 = otherSession.Instantiate(c1.Id);

                        Assert.Equal(c1.Strategy.ObjectVersion, otherC1.Strategy.ObjectVersion);
                        Assert.NotEqual(initialObjectVersion, otherC1.Strategy.ObjectVersion);
                        Assert.NotEqual(changedObjectVersion, otherC1.Strategy.ObjectVersion);
                    }
                }
            }
        }

        [Fact]
        public void SaveDifferentCultureInfos()
        {
            foreach (var init in this.Inits)
            {
                init();

                var writeCultureInfo = new CultureInfo("en-US");
                var readCultureInfo = new CultureInfo("en-GB");

                CultureInfo.CurrentCulture = writeCultureInfo;
                CultureInfo.CurrentUICulture = writeCultureInfo;

                using (var session = this.CreatePopulation().CreateSession())
                {
                    this.Populate(session);

                    var stringWriter = new StringWriter();
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        session.Database.Save(writer);
                    }

                    CultureInfo.CurrentCulture = readCultureInfo;
                    CultureInfo.CurrentUICulture = readCultureInfo;

                    var stringReader = new StringReader(stringWriter.ToString());
                    using (var reader = XmlReader.Create(stringReader))
                    {
                        var savePopulation = this.CreatePopulation();
                        savePopulation.Load(reader);

                        var saveSession = savePopulation.CreateSession();

                        this.AssertPopulation(saveSession);

                        saveSession.Rollback();
                    }
                }
            }
        }

        [Fact]
        public void LoadBinary()
        {
            foreach (var init in this.Inits)
            {
                init();

                var otherPopulation = this.CreatePopulation();
                var otherSession = otherPopulation.CreateSession();

                try
                {
                    this.c1A = C1.Create(otherSession);
                    this.c1B = C1.Create(otherSession);
                    this.c1C = C1.Create(otherSession);

                    this.c1A.C1AllorsBinary = new byte[0];
                    this.c1B.C1AllorsBinary = new byte[] { 1, 2, 3, 4 };
                    this.c1C.C1AllorsBinary = null;

                    otherSession.Commit();

                    var stringWriter = new StringWriter();
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        otherPopulation.Save(writer);
                    }

                    var xml = stringWriter.ToString();

                    var stringReader = new StringReader(stringWriter.ToString());
                    using (var reader = XmlReader.Create(stringReader))
                    {
                        this.Population.Load(reader);
                    }

                    using (var session = this.Population.CreateSession())
                    {
                        var c1ACopy = C1.Instantiate(session, this.c1A.Strategy.ObjectId);
                        var c1BCopy = C1.Instantiate(session, this.c1B.Strategy.ObjectId);
                        var c1CCopy = C1.Instantiate(session, this.c1C.Strategy.ObjectId);

                        Assert.Equal(this.c1A.C1AllorsBinary, c1ACopy.C1AllorsBinary);
                        Assert.Equal(this.c1B.C1AllorsBinary, c1BCopy.C1AllorsBinary);
                        Assert.Equal(this.c1C.C1AllorsBinary, c1CCopy.C1AllorsBinary);
                    }
                }
                finally
                {
                    otherSession.Commit();
                }
            }
        }


        [Fact]
        public void CantLoadObjects()
        {
            foreach (var init in this.Inits)
            {
                init();

                var xml =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<allors>
  <population version=""1"">
    <objects>
      <database>
        <ot i=""7041c691d89646288f501c24f5d03414"">1:0</ot>
        <ot i=""71000000000000000000000000000000"">3:0</ot>
        <ot i=""72c07e8a03f54da8ab37236333d4f74e"">2:0</ot>
      </database>
    </objects>
  </population>
</allors>";
                var notLoadedEventArgs = new List<ObjectNotLoadedEventArgs>();
                this.Population.ObjectNotLoaded += (o, args) =>
                    notLoadedEventArgs.Add(args);

                var stringReader = new StringReader(xml);
                using (var reader = XmlReader.Create(stringReader))
                {
                    this.Population.Load(reader);
                }

                Assert.Single(notLoadedEventArgs);
                var notLoadedEventArg = notLoadedEventArgs.First();
                Assert.Equal(3, notLoadedEventArg.ObjectId);
                Assert.Equal(new Guid("71000000000000000000000000000000"), notLoadedEventArg.ObjectTypeId);

                using (var session = this.Population.CreateSession())
                {
                    this.c1A = (C1)session.Instantiate(1);
                    this.c2A = (C2)session.Instantiate(2);

                    Assert.NotNull(this.c1A);
                    Assert.NotNull(this.c2A);
                }
            }
        }

        [Fact]
        public void CantLoadUnitRelation()
        {
            foreach (var init in this.Inits)
            {
                init();

                var xml =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<allors>
  <population version=""1"">
    <objects>
      <database>
        <ot i=""7041c691d89646288f501c24f5d03414"">1:0,2:0,3:0,4:0</ot>
        <ot i=""72c07e8a03f54da8ab37236333d4f74e"">5:0,6:0,7:0,8:0</ot>
      </database>
    </objects>
    <relations>
      <database>
        <rtu i=""207138608abd4d718ccc2b4d1b88bce3"">
          <r a=""1"">A String</r>
        </rtu>
        <rtu i=""40000000000000000000000000000000"">
          <r a=""2"">Oops</r>
        </rtu>
        <rtu i=""b4ee673fbba04e249cda3cf993c79a0a"">
          <r a=""3"">true</r>
        </rtu>
        <rtu i=""cef13620b7d74bfe8d3bc0f826da5989"">
          <r a=""1"">537f6823-d22c-4b3b-ab3c-e15a6b61b9d6</r>
        </rtu>
      </database>
    </relations>
  </population>
</allors>";

                var notLoadedEventArgs = new List<RelationNotLoadedEventArgs>();
                this.Population.RelationNotLoaded += (o, args) =>
                    notLoadedEventArgs.Add(args);

                var stringReader = new StringReader(xml);
                using (var reader = XmlReader.Create(stringReader))
                {
                    this.Population.Load(reader);
                }

                Assert.Single(notLoadedEventArgs);
                var notLoadedEventArg = notLoadedEventArgs.First();
                Assert.Equal(2, notLoadedEventArg.AssociationId);
                Assert.Equal(new Guid("40000000000000000000000000000000"), notLoadedEventArg.RelationTypeId);
                Assert.Equal("Oops", notLoadedEventArg.RoleContents);

                using (var session = this.Population.CreateSession())
                {
                    this.c1A = (C1)session.Instantiate(1);
                    this.c1C = (C1)session.Instantiate(3);

                    Assert.Equal("A String", this.c1A.C1AllorsString);
                    Assert.Equal(true, this.c1C.C1AllorsBoolean);
                    Assert.Equal(new Guid("537f6823-d22c-4b3b-ab3c-e15a6b61b9d6"), this.c1A.C1AllorsUnique);
                }
            }
        }

        [Fact]
        public void CantLoadUnitRole()
        {
            foreach (var init in this.Inits)
            {
                init();

                var xml =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<allors>
  <population version=""1"">
    <objects>
      <database>
        <ot i=""7041c691d89646288f501c24f5d03414"">1:0,2:0,3:0,4:0</ot>
        <ot i=""72c07e8a03f54da8ab37236333d4f74e"">5:0,6:0,7:0,8:0</ot>
      </database>
    </objects>
    <relations>
      <database>
        <rtu i=""207138608abd4d718ccc2b4d1b88bce3"">
            <r a=""1"">A String</r>
        </rtu>
        <rtu i=""87eb0d1973a74aaeaeed66dc9163233c"">
            <r a=""99"">1.1</r>
        </rtu>
        <rtu i=""b4ee673fbba04e249cda3cf993c79a0a"">
            <r a=""1"">true</r>
        </rtu>
        <rtu i=""cef13620b7d74bfe8d3bc0f826da5989"">
          <r a=""1"">537f6823-d22c-4b3b-ab3c-e15a6b61b9d6</r>
        </rtu>
     </database>
    </relations>
  </population>
</allors>";

                var notLoadedEventArgs = new List<RelationNotLoadedEventArgs>();
                this.Population.RelationNotLoaded += (o, args) =>
                    notLoadedEventArgs.Add(args);

                var stringReader = new StringReader(xml);
                using (var reader = XmlReader.Create(stringReader))
                {
                    this.Population.Load(reader);
                }

                Assert.Single(notLoadedEventArgs);
                var notLoadedEventArg = notLoadedEventArgs.First();
                Assert.Equal(99, notLoadedEventArg.AssociationId);
                Assert.Equal(new Guid("87eb0d1973a74aaeaeed66dc9163233c"), notLoadedEventArg.RelationTypeId);
                Assert.Equal("1.1", notLoadedEventArg.RoleContents);

                using (var session = this.Population.CreateSession())
                {
                    this.c1A = (C1)session.Instantiate(1);

                    Assert.Equal("A String", this.c1A.C1AllorsString);
                    Assert.Equal(true, this.c1A.C1AllorsBoolean);
                    Assert.Equal(new Guid("537f6823-d22c-4b3b-ab3c-e15a6b61b9d6"), this.c1A.C1AllorsUnique);
                }
            }
        }


        [Fact]
        public void CantLoadCompositeRelation()
        {
            foreach (var init in this.Inits)
            {
                init();

                var xml =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<allors>
  <population version=""1"">
    <objects>
      <database>
        <ot i=""7041c691d89646288f501c24f5d03414"">1:0,2:0,3:0,4:0</ot>
        <ot i=""72c07e8a03f54da8ab37236333d4f74e"">5:0,6:0,7:0,8:0</ot>
      </database>
    </objects>
    <relations>
      <database>
        <rtc i=""2ff1c9ba0017466e9f11776086e6d0b0"">
          <r a=""1"">2</r>
        </rtc>
        <rtc i=""30000000000000000000000000000000"">
          <r a=""2"">3</r>
        </rtc>
        <rtc i=""4c77650277d745d9b10162dee27c0c2e"">
          <r a=""3"">4</r>
        </rtc>
        <rtc i=""ab6d11ccec86482888752e9a779ba627"">
          <r a=""1"">4</r>
        </rtc>
    </database>
    </relations>
  </population>
</allors>";

                var notLoadedEventArgs = new List<RelationNotLoadedEventArgs>();
                this.Population.RelationNotLoaded += (o, args) =>
                    notLoadedEventArgs.Add(args);

                var stringReader = new StringReader(xml);
                using (var reader = XmlReader.Create(stringReader))
                {
                    this.Population.Load(reader);
                }

                Assert.Single(notLoadedEventArgs);
                var notLoadedEventArg = notLoadedEventArgs.First();
                Assert.Equal(2, notLoadedEventArg.AssociationId);
                Assert.Equal(new Guid("30000000000000000000000000000000"), notLoadedEventArg.RelationTypeId);
                Assert.Equal("3", notLoadedEventArg.RoleContents);

                using (var session = this.Population.CreateSession())
                {
                    this.c1A = (C1)session.Instantiate(1);
                    this.c1B = (C1)session.Instantiate(2);
                    this.c1C = (C1)session.Instantiate(3);
                    this.c1D = (C1)session.Instantiate(4);

                    Assert.Single(this.c1A.C1C1many2manies);
                    Assert.Contains(this.c1B, this.c1A.C1C1many2manies);
                    Assert.Equal(this.c1D, this.c1C.C1C1one2one);
                    Assert.Single(this.c1A.C1C1one2manies);
                    Assert.Contains(this.c1D, this.c1A.C1C1one2manies);
                }
            }
        }


        [Fact]
        public void CantLoadCompositeRole()
        {
            foreach (var init in this.Inits)
            {
                init();

                var xml =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<allors>
  <population version=""1"">
    <objects>
      <database>
        <ot i=""7041c691d89646288f501c24f5d03414"">1:0,2:0,3:0,4:0</ot>
        <ot i=""72c07e8a03f54da8ab37236333d4f74e"">5:0,6:0,7:0,8:0</ot>
      </database>
    </objects>
    <relations>
      <database>
        <rtc i=""2ff1c9ba0017466e9f11776086e6d0b0"">
          <r a=""1"">2</r>
        </rtc>
        <rtc i=""2cd8b843-f1f5-413d-9d6d-0d2b9b3c5cf6"">
          <r a=""99"">3</r>
        </rtc>
        <rtc i=""4c776502-77d7-45d9-b101-62dee27c0c2e"">
          <r a=""3"">4</r>
        </rtc>
        <rtc i=""ab6d11ccec86482888752e9a779ba627"">
          <r a=""1"">4</r>
        </rtc>
    </database>
    </relations>
  </population>
</allors>";

                var notLoadedEventArgs = new List<RelationNotLoadedEventArgs>();
                this.Population.RelationNotLoaded += (o, args) =>
                    notLoadedEventArgs.Add(args);

                var stringReader = new StringReader(xml);
                using (var reader = XmlReader.Create(stringReader))
                {
                    this.Population.Load(reader);
                }

                Assert.Single(notLoadedEventArgs);
                var notLoadedEventArg = notLoadedEventArgs.First();
                Assert.Equal(99, notLoadedEventArg.AssociationId);
                Assert.Equal(new Guid("2cd8b843-f1f5-413d-9d6d-0d2b9b3c5cf6"), notLoadedEventArg.RelationTypeId);
                Assert.Equal("3", notLoadedEventArg.RoleContents);

                using (var session = this.Population.CreateSession())
                {
                    this.c1A = (C1)session.Instantiate(1);
                    this.c1B = (C1)session.Instantiate(2);
                    this.c1C = (C1)session.Instantiate(3);
                    this.c1D = (C1)session.Instantiate(4);

                    Assert.Single(this.c1A.C1C1many2manies);
                    Assert.Contains(this.c1B, this.c1A.C1C1many2manies);
                    Assert.Equal(this.c1D, this.c1C.C1C1one2one);
                    Assert.Single(this.c1A.C1C1one2manies);
                    Assert.Contains(this.c1D, this.c1A.C1C1one2manies);
                }
            }
        }


        protected abstract IDatabase CreatePopulation();

        private static string DoSave(IDatabase otherPopulation)
        {
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                otherPopulation.Save(writer);
            }

            return stringWriter.ToString();
        }

        private static void DoLoad(IDatabase database, string xml)
        {
            var stringReader = new StringReader(xml);
            using (var reader = XmlReader.Create(stringReader))
            {
                database.Load(reader);
            }
        }

        private void AssertPopulation(ISession session)
        {
            Assert.Equal(4, this.GetExtent(session, C1.Meta.ObjectType).Length);
            Assert.Equal(4, this.GetExtent(session, C2.Meta.ObjectType).Length);
            Assert.Equal(4, this.GetExtent(session, C3.Meta.ObjectType).Length);
            Assert.Equal(4, this.GetExtent(session, C4.Meta.ObjectType).Length);

            var c1ACopy = C1.Instantiate(session, this.c1A.Strategy.ObjectId);
            var c1BCopy = C1.Instantiate(session, this.c1B.Strategy.ObjectId);
            var c1CCopy = C1.Instantiate(session, this.c1C.Strategy.ObjectId);
            var c1DCopy = C1.Instantiate(session, this.c1D.Strategy.ObjectId);
            var c2ACopy = C2.Instantiate(session, this.c2A.Strategy.ObjectId);
            var c2BCopy = C2.Instantiate(session, this.c2B.Strategy.ObjectId);
            var c2CCopy = C2.Instantiate(session, this.c2C.Strategy.ObjectId);
            var c2DCopy = C2.Instantiate(session, this.c2D.Strategy.ObjectId);
            var c3ACopy = C3.Instantiate(session, this.c3A.Strategy.ObjectId);
            var c3BCopy = C3.Instantiate(session, this.c3B.Strategy.ObjectId);
            var c3CCopy = C3.Instantiate(session, this.c3C.Strategy.ObjectId);
            var c3DCopy = C3.Instantiate(session, this.c3D.Strategy.ObjectId);
            var c4ACopy = C4.Instantiate(session, this.c4A.Strategy.ObjectId);
            var c4BCopy = C4.Instantiate(session, this.c4B.Strategy.ObjectId);
            var c4CCopy = C4.Instantiate(session, this.c4C.Strategy.ObjectId);
            var c4DCopy = C4.Instantiate(session, this.c4D.Strategy.ObjectId);

            IObject[] everyC1 = { c1ACopy, c1BCopy, c1CCopy, c1DCopy };
            IObject[] everyC2 = { c2ACopy, c2BCopy, c2CCopy, c2DCopy };
            IObject[] everyC3 = { c3ACopy, c3BCopy, c3CCopy, c3DCopy };
            IObject[] everyC4 = { c4ACopy, c4BCopy, c4CCopy, c4DCopy };
            IObject[] everyObject =
                                    {
                                        c1ACopy, c1BCopy, c1CCopy, c1DCopy, c2ACopy, c2BCopy, c2CCopy, c2DCopy, c3ACopy,
                                        c3BCopy, c3CCopy, c3DCopy, c4ACopy, c4BCopy, c4CCopy, c4DCopy
                                    };

            foreach (var allorsObject in everyObject)
            {
                Assert.NotNull(allorsObject);
            }

            if (this.EmptyStringIsNull)
            {
                Assert.False(c1ACopy.ExistC1AllorsString);
            }
            else
            {
                Assert.Equal(this.c1A.C1AllorsString, string.Empty);
            }

            Assert.Equal(this.c1A.C1AllorsInteger, -1);
            Assert.Equal(1.1m, this.c1A.C1AllorsDecimal);
            Assert.Equal(1.1d, this.c1A.C1AllorsDouble);
            Assert.True(this.c1A.C1AllorsBoolean);
            Assert.Equal(this.c1A.C1AllorsDateTime, new DateTime(1973, 3, 27, 12, 1, 2, 3, DateTimeKind.Utc));
            Assert.Equal(this.c1A.C1AllorsUnique, new Guid(GuidString));

            Assert.Equal(this.c1A.C1AllorsBinary, new byte[0]);
            Assert.Equal(this.c1B.C1AllorsBinary, new byte[] { 0, 1, 2, 3 });
            Assert.Null(this.c1C.C1AllorsBinary);

            Assert.Equal("a1", c2ACopy.C1WhereC1C2one2one.C1AllorsString);
            Assert.Equal("a1", c2ACopy.C1WhereC1C2one2many.C1AllorsString);
            Assert.Equal("a1", c2BCopy.C1WhereC1C2one2many.C1AllorsString);

            Assert.Equal("c3a", c3ACopy.I34AllorsString);
            Assert.Equal("c4a", c4ACopy.I34AllorsString);

            Assert.Equal(2, c2ACopy.C1sWhereC1C2many2one.Count);
            Assert.Empty(c2BCopy.C1sWhereC1C2many2one);
            Assert.Single(c2ACopy.C1sWhereC1C2many2many);
            Assert.Single(c2BCopy.C1sWhereC1C2many2many);

            foreach (S1234 allorsObject in everyObject)
            {
                Assert.Equal(everyObject.Length, allorsObject.S1234many2manies.Count);
                foreach (S1234 addObject in everyObject)
                {
                    var objects = allorsObject.S1234many2manies.ToArray();
                    Assert.Contains(addObject, objects);
                }
            }
        }

        private void Populate(ISession session)
        {
            this.c1A = C1.Create(session);
            this.c1B = C1.Create(session);
            this.c1C = C1.Create(session);
            this.c1D = C1.Create(session);
            this.c2A = C2.Create(session);
            this.c2B = C2.Create(session);
            this.c2C = C2.Create(session);
            this.c2D = C2.Create(session);
            this.c3A = C3.Create(session);
            this.c3B = C3.Create(session);
            this.c3C = C3.Create(session);
            this.c3D = C3.Create(session);
            this.c4A = C4.Create(session);
            this.c4B = C4.Create(session);
            this.c4C = C4.Create(session);
            this.c4D = C4.Create(session);

            IObject[] allObjects =
                                   {
                                       this.c1A, this.c1B, this.c1C, this.c1D, this.c2A, this.c2B, this.c2C, this.c2D,
                                       this.c3A, this.c3B, this.c3C, this.c3D, this.c4A, this.c4B, this.c4C, this.c4D
                                   };

            this.c1A.C1AllorsString = string.Empty; // emtpy string
            this.c1A.C1AllorsInteger = -1;
            this.c1A.C1AllorsDecimal = 1.1m;
            this.c1A.C1AllorsDouble = 1.1d;
            this.c1A.C1AllorsBoolean = true;
            this.c1A.C1AllorsDateTime = new DateTime(1973, 3, 27, 12, 1, 2, 3, DateTimeKind.Utc);
            this.c1A.C1AllorsUnique = new Guid(GuidString);
            this.c1A.C1AllorsBinary = new byte[0];

            this.c1B.C1AllorsString = "a1";
            this.c1B.C1AllorsBinary = new byte[] { 0, 1, 2, 3 };
            this.c1B.C1C2one2one = this.c2A;
            this.c1B.C1C2many2one = this.c2A;
            this.c1C.C1C2many2one = this.c2A;
            this.c1B.AddC1C2one2many(this.c2A);
            this.c1B.AddC1C2one2many(this.c2B);
            this.c1B.AddC1C2one2many(this.c2C);
            this.c1B.AddC1C2one2many(this.c2D);
            this.c1B.AddC1C2many2many(this.c2A);
            this.c1B.AddC1C2many2many(this.c2B);
            this.c1B.AddC1C2many2many(this.c2C);
            this.c1B.AddC1C2many2many(this.c2D);

            this.c1C.C1AllorsString = "a2";
            this.c1C.C1AllorsBinary = null;

            this.c3A.I34AllorsString = "c3a";
            this.c4A.I34AllorsString = "c4a";

            foreach (S1234 allorsObject in allObjects)
            {
                foreach (S1234 addObject in allObjects)
                {
                    allorsObject.AddS1234many2many(addObject);
                }
            }

            session.Commit();
        }

        private static void Dump(IDatabase population)
        {
            using (var stream = File.Create(@"population.xml"))
            {
                using (var writer = XmlWriter.Create(stream))
                {
                    population.Save(writer);
                }
            }
        }

        private IObject[] GetExtent(ISession session, IComposite objectType)
        {
            return session.Extent(objectType);
        }
    }
}