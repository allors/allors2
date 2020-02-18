// <copyright file="SerializationTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
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
    using Allors.Serialization;
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

        protected IDatabase Database => this.Profile.Database;

        protected ISession Session => this.Profile.Session;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

        public abstract void Dispose();

        [Fact(Skip = "WIP")]
        public void SaveEmpty()
        {
            foreach (var init in this.Inits)
            {
                init();

                var populationData = this.Database.Save();
                var classesData = populationData.ToArray();

                Assert.Empty(classesData);

                foreach (var classData in classesData)
                {
                    Assert.Empty(classData);
                }
            }
        }

        [Fact(Skip = "WIP")]
        public void Save()
        {
            foreach (var init in this.Inits)
            {
                init();

                using (var session = this.Database.CreateSession())
                {
                    this.Populate(session);
                }

                var populationData = this.Database.Save();
                var classesData = populationData.ToArray();

                Assert.Equal(4, classesData.Length);

                var classes = classesData.Select(v => v.Class).Distinct().ToArray();

                Assert.Equal(4, classes.Length);

                Assert.Contains(M.C1.Class, classes);
                Assert.Contains(M.C2.Class, classes);
                Assert.Contains(M.C3.Class, classes);
                Assert.Contains(M.C4.Class, classes);

                foreach (var classData in classesData)
                {
                    var objectsData = classData.ToArray();
                    Assert.Equal(4, objectsData.Length);

                    var versions = objectsData.Select(v => v.Version).ToArray();
                    Assert.True(versions.All(v => v == 1));

                    if (classData.Class.Equals(M.C1.Class))
                    {
                        foreach (var objectData in objectsData)
                        {
                            switch (objectData.Id)
                            {
                                case 1:
                                    break;
                                case 2:
                                    break;
                                case 3:
                                    break;
                                case 4:
                                    break;
                                default:
                                    Assert.False(true);
                                    break;
                            }
                        }
                    }

                    if (classData.Class.Equals(M.C2.Class))
                    {

                    }

                    if (classData.Class.Equals(M.C3.Class))
                    {

                    }

                    if (classData.Class.Equals(M.C4.Class))
                    {

                    }
                }
            }
        }

        protected abstract IDatabase CreatePopulation();

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
                                        c3BCopy, c3CCopy, c3DCopy, c4ACopy, c4BCopy, c4CCopy, c4DCopy,
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
                Assert.Equal(string.Empty, c1ACopy.C1AllorsString);
            }

            Assert.Equal(-1, c1ACopy.C1AllorsInteger);
            Assert.Equal(1.1m, c1ACopy.C1AllorsDecimal);
            Assert.Equal(1.1d, c1ACopy.C1AllorsDouble);
            Assert.True(c1ACopy.C1AllorsBoolean);
            Assert.Equal(new DateTime(1973, 3, 27, 12, 1, 2, 3, DateTimeKind.Utc), c1ACopy.C1AllorsDateTime);
            Assert.Equal(new Guid(GuidString), c1ACopy.C1AllorsUnique);

            Assert.Equal(new byte[0], c1ACopy.C1AllorsBinary);
            Assert.Equal(new byte[] { 0, 1, 2, 3 }, c1BCopy.C1AllorsBinary);
            Assert.Null(c1CCopy.C1AllorsBinary);

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
                                       this.c3A, this.c3B, this.c3C, this.c3D, this.c4A, this.c4B, this.c4C, this.c4D,
                                   };

            this.c1A.C1AllorsString = string.Empty; // emtpy string
            this.c1A.C1AllorsInteger = -1;
            this.c1A.C1AllorsDecimal = 1.1m;
            this.c1A.C1AllorsDouble = 1.1d;
            this.c1A.C1AllorsBoolean = true;
            this.c1A.C1AllorsDateTime = new DateTime(1973, 3, 27, 12, 1, 2, 3, DateTimeKind.Utc);
            this.c1A.C1AllorsUnique = new Guid(GuidString);
            this.c1A.C1AllorsBinary = Array.Empty<byte>();

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

        private IObject[] GetExtent(ISession session, IComposite objectType) => session.Extent(objectType);
    }
}
