
// <copyright file="SchemaTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters
{
    using System;

    using Allors;
    using Allors.Meta;

    using Xunit;

    using IDatabase = IDatabase;

    public enum ColumnTypes
    {
        ObjectId,
        TypeId,
        CacheId,

        Binary,
        Boolean,
        Decimal,
        Float,
        Integer,
        String,
        Unique,
    }

    public abstract class SchemaTest : IDisposable
    {
        protected Domain domain;

        protected virtual bool DetectBinarySizedDifferences => true;

        protected abstract IProfile Profile { get; }

        protected ISession Session => this.Profile.Session;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

        // TODO: Save Load (remove and add relations)

        public abstract void Dispose();

        [Fact]
        public void Objects()
        {
            //this.DropTable("allors", "_o");

            //this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

            //var Database = this.CreateDatabase(this.domain.MetaPopulation, true);
            //ISession session = Database.CreateSession();
            //session.Rollback();

            //var tableName = "_o";
            //Assert.True(this.ExistTable("allors", tableName));
            //Assert.Equal(3, this.ColumnCount("allors", tableName));
            //Assert.True(this.ExistColumn("allors", tableName, "o", ColumnTypes.ObjectId));
            //Assert.True(this.ExistColumn("allors", tableName, "t", ColumnTypes.TypeId));
            //Assert.True(this.ExistColumn("allors", tableName, "c", ColumnTypes.CacheId));

            //this.DropTable("allors", "_o");

            //Database = this.CreateDatabase(this.domain.MetaPopulation, false);

            //var exceptionThrown = false;
            //try
            //{
            //    Database.CreateSession();
            //}
            //catch
            //{
            //    exceptionThrown = true;
            //}

            //Assert.True(exceptionThrown);
        }

        //[Fact]
        //[ExpectedException]
        //public void InitInvalidDomain()
        //{
        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

        //    new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();

        //    this.CreateDatabase(this.domain.MetaPopulation, true);
        //}

        //[Fact]
        //public void SystemTables()
        //{
        //    Assert.True(this.IsUnique("_O", "T"));
        //    Assert.True(this.IsInteger("_O", "C"));

        //    Assert.True(this.ExistPrimaryKey("_O", "O"));
        //    Assert.False(this.ExistIndex("_O", "T"));
        //    Assert.False(this.ExistIndex("_O", "C"));
        //}

        //[Fact]
        //public void ValidateBinaryRelationDifferentSize()
        //{
        //    if (this.DetectBinarySizedDifferences)
        //    {
        //        this.DropTable("C1");
        //        this.DropTable("C2");

        //        var environment = new MetaPopulation();
        //        var core = Repository.Core(environment);
        //        this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //        this.domain.AddDirectSuperdomain(core);

        //        var c1 = this.CreateClass("C1");
        //        this.CreateClass("C2");

        //        var allorsBinary = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.BinaryId);
        //        new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).WithObjectTypes(c1, allorsBinary).WithSize(200).Build();

        //        this.CreateDatabase(this.domain.MetaPopulation, true);

        //        environment = new MetaPopulation();
        //        core = Repository.Core(environment);
        //        this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //        this.domain.AddDirectSuperdomain(core);

        //        c1 = this.CreateClass("C1");
        //        this.CreateClass("C2");

        //        allorsBinary = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.BinaryId);
        //        var c1RelationType = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).WithObjectTypes(c1, allorsBinary).Build();

        //        // Different Size
        //        c1RelationType.RoleType.Size = 300;

        //        var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //        var validationErrors = this.GetSchemaValidation(Database);

        //        var tableErrors = validationErrors.TableErrors;

        //        Assert.Equal(1, tableErrors.Length);
        //        Assert.Equal(SchemaValidationErrorKind.Incompatible, tableErrors[0].Kind);

        //        var error = tableErrors[0];

        //        Assert.Equal(null, error.ObjectType);
        //        Assert.Equal(null, error.RelationType);
        //        Assert.Equal(c1RelationType.RoleType, error.Role);

        //        Assert.Equal("c1", error.TableName);
        //        Assert.Equal("allorsbinary", error.ColumnName);
        //    }
        //}

        //[Fact]
        //public void ValidateDecimalRelationDifferentPrecision()
        //{
        //    this.DropTable("C1");
        //    this.DropTable("C2");

        //    var environment = new MetaPopulation();
        //    var core = Repository.Core(environment);
        //    this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.AddDirectSuperdomain(core);

        //    var c1 = this.CreateClass("C1");
        //    this.CreateClass("C2");

        //    var c1RelationType = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    c1RelationType.AssociationType.ObjectType = c1;
        //    c1RelationType.RoleType.ObjectType = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.DecimalId);
        //    c1RelationType.RoleType.Precision = 10;
        //    c1RelationType.RoleType.Scale = 2;

        //    this.CreateDatabase(this.domain.MetaPopulation, true);

        //    environment = new MetaPopulation();
        //    core = Repository.Core(environment);
        //    this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.AddDirectSuperdomain(core);

        //    c1 = this.CreateClass("C1");
        //    this.CreateClass("C2");

        //    c1RelationType = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    c1RelationType.AssociationType.ObjectType = c1;
        //    c1RelationType.RoleType.ObjectType = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.DecimalId);
        //    c1RelationType.RoleType.Precision = 10;
        //    c1RelationType.RoleType.Scale = 2;

        //    // Different precision
        //    c1RelationType.RoleType.Precision = 11;

        //    var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    var validationErrors = this.GetSchemaValidation(Database);

        //    var tableErrors = validationErrors.TableErrors;

        //    Assert.Equal(1, tableErrors.Length);
        //    Assert.Equal(SchemaValidationErrorKind.Incompatible, tableErrors[0].Kind);

        //    var error = tableErrors[0];

        //    Assert.Equal(null, error.ObjectType);
        //    Assert.Equal(null, error.RelationType);
        //    Assert.Equal(c1RelationType.RoleType, error.Role);

        //    Assert.Equal("c1", error.TableName);
        //    Assert.Equal("allorsdecimal", error.ColumnName);
        //}

        //[Fact]
        //public void ValidateDecimalRelationDifferentScale()
        //{
        //    this.DropTable("C1");
        //    this.DropTable("C2");

        //    var environment = new MetaPopulation();
        //    var core = Repository.Core(environment);
        //    this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.AddDirectSuperdomain(core);

        //    var c1 = this.CreateClass("C1");
        //    this.CreateClass("C2");

        //    var c1RelationType = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    c1RelationType.AssociationType.ObjectType = c1;
        //    c1RelationType.RoleType.ObjectType = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.DecimalId);
        //    c1RelationType.RoleType.Precision = 10;
        //    c1RelationType.RoleType.Scale = 2;

        //    this.CreateDatabase(this.domain.MetaPopulation, true);

        //    environment = new MetaPopulation();
        //    core = Repository.Core(environment);
        //    this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.AddDirectSuperdomain(core);

        //    c1 = this.CreateClass("C1");
        //    this.CreateClass("C2");

        //    c1RelationType = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    c1RelationType.AssociationType.ObjectType = c1;
        //    c1RelationType.RoleType.ObjectType = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.DecimalId);
        //    c1RelationType.RoleType.Precision = 10;
        //    c1RelationType.RoleType.Scale = 2;

        //    // Different scale
        //    c1RelationType.RoleType.Scale = 3;

        //    var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    var validationErrors = this.GetSchemaValidation(Database);

        //    var tableErrors = validationErrors.TableErrors;

        //    Assert.Equal(1, tableErrors.Length);
        //    Assert.Equal(SchemaValidationErrorKind.Incompatible, tableErrors[0].Kind);

        //    var error = tableErrors[0];

        //    Assert.Equal(null, error.ObjectType);
        //    Assert.Equal(null, error.RelationType);
        //    Assert.Equal(c1RelationType.RoleType, error.Role);

        //    Assert.Equal("c1", error.TableName);
        //    Assert.Equal("allorsdecimal", error.ColumnName);
        //}

        //[Fact]
        //public void ValidateNewConcreteClass()
        //{
        //    this.DropTable("C1");
        //    this.DropTable("C2");

        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.Name = "MyDomain";

        //    this.CreateClass("C1");

        //    this.CreateDatabase(this.domain.MetaPopulation, true);

        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.Name = "MyDomain";

        //    this.CreateClass("C1");

        //    // Extra class
        //    this.CreateClass("C2");

        //    var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    var validationErrors = this.GetSchemaValidation(Database);

        //    var tableErrors = validationErrors.TableErrors;

        //    Assert.Equal(1, tableErrors.Length);

        //    var error = tableErrors[0];

        //    Assert.Equal("c2", error.TableName);
        //    Assert.Equal(SchemaValidationErrorKind.Missing, tableErrors[0].Kind);
        //}

        //[Fact]
        //public void ValidateNewInterfaceInheritanceWithBooleanRelation()
        //{
        //    this.DropTable("C1");
        //    this.DropTable("C2");

        //    var environment = new MetaPopulation();
        //    var core = Repository.Core(environment);
        //    this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.AddDirectSuperdomain(core);

        //    var c1 = this.CreateClass("C1");
        //    var c2 = this.CreateClass("C2");

        //    var i12 = this.CreateInterface("I12");

        //    var i12AllorsString = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    i12AllorsString.AssociationType.ObjectType = i12;
        //    i12AllorsString.RoleType.ObjectType = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.BooleanId);

        //    new InheritanceBuilder(this.domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(i12).Build();

        //    this.CreateDatabase(this.domain.MetaPopulation, true);

        //    environment = new MetaPopulation();
        //    core = Repository.Core(environment);
        //    this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.AddDirectSuperdomain(core);

        //    c1 = this.CreateClass("C1");
        //    c2 = this.CreateClass("C2");

        //    i12 = this.CreateInterface("I12");

        //    i12AllorsString = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    i12AllorsString.AssociationType.ObjectType = i12;
        //    i12AllorsString.RoleType.ObjectType = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.BooleanId);

        //    new InheritanceBuilder(this.domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(i12).Build();

        //    // Extra inheritance
        //    new InheritanceBuilder(this.domain, Guid.NewGuid()).WithSubtype(c2).WithSupertype(i12).Build();

        //    var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    var validationErrors = this.GetSchemaValidation(Database);

        //    var tableErrors = validationErrors.TableErrors;

        //    Assert.Equal(1, tableErrors.Length);

        //    var error = tableErrors[0];

        //    Assert.Equal("c2", error.TableName);
        //    Assert.Equal("allorsboolean", error.ColumnName);
        //    Assert.Equal(SchemaValidationErrorKind.Missing, tableErrors[0].Kind);
        //}

        //[Fact]
        //public void ValidateNewMany2ManyRelation()
        //{
        //    this.DropTable("C1");
        //    this.DropTable("C2");

        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

        //    var c1 = this.CreateClass("C1");
        //    var c2 = this.CreateClass("C2");

        //    this.CreateDatabase(this.domain.MetaPopulation, true);

        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

        //    c1 = this.CreateClass("C1");
        //    c2 = this.CreateClass("C2");

        //    // Extra relation
        //    var fromC1ToC2 = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).WithCardinality(Multiplicity.ManyToMany).Build();
        //    fromC1ToC2.AssociationType.ObjectType = c1;
        //    fromC1ToC2.RoleType.ObjectType = c2;

        //    var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    var validationErrors = this.GetSchemaValidation(Database);

        //    var tableErros = validationErrors.TableErrors;

        //    Assert.Equal(1, tableErros.Length);
        //    Assert.Equal(SchemaValidationErrorKind.Missing, tableErros[0].Kind);

        //    var error = tableErros[0];

        //    Assert.Equal(null, error.ObjectType);
        //    Assert.Equal(fromC1ToC2, error.RelationType);
        //    Assert.Equal(null, error.Role);

        //    Assert.Equal("c1c2", error.TableName);
        //    Assert.Equal(null, error.ColumnName);
        //}

        //[Fact]
        //public void ValidateNewMany2OneRelation()
        //{
        //    this.DropTable("C1");
        //    this.DropTable("C2");

        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

        //    var c1 = this.CreateClass("C1");
        //    var c2 = this.CreateClass("C2");

        //    this.CreateDatabase(this.domain.MetaPopulation, true);

        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

        //    c1 = this.CreateClass("C1");
        //    c2 = this.CreateClass("C2");

        //    // Extra relation
        //    var fromC1ToC2 = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).WithCardinality(Multiplicity.ManyToOne).Build();
        //    fromC1ToC2.AssociationType.ObjectType = c1;
        //    fromC1ToC2.RoleType.ObjectType = c2;

        //    var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    var validationErrors = this.GetSchemaValidation(Database);
        //    var tableErros = validationErrors.TableErrors;

        //    Assert.Equal(1, tableErros.Length);
        //    Assert.Equal(SchemaValidationErrorKind.Missing, tableErros[0].Kind);

        //    var error = tableErros[0];

        //    Assert.Equal(null, error.ObjectType);
        //    Assert.Equal(null, error.RelationType);
        //    Assert.Equal(fromC1ToC2.RoleType, error.Role);
        //    Assert.Equal("c1", error.TableName);
        //    Assert.Equal("c2", error.ColumnName);
        //}

        //[Fact]
        //public void ValidateNewOne2ManyRelation()
        //{
        //    this.DropTable("C1");
        //    this.DropTable("C2");

        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

        //    var c1 = this.CreateClass("C1");
        //    var c2 = this.CreateClass("C2");

        //    this.CreateDatabase(this.domain.MetaPopulation, true);

        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

        //    c1 = this.CreateClass("C1");
        //    c2 = this.CreateClass("C2");

        //    // extra relation
        //    var fromC1ToC2 = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).WithCardinality(Multiplicity.OneToMany).Build();
        //    fromC1ToC2.AssociationType.ObjectType = c1;
        //    fromC1ToC2.RoleType.ObjectType = c2;

        //    var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    var validationErrors = this.GetSchemaValidation(Database);

        //    var tableErrors = validationErrors.TableErrors;

        //    Assert.Equal(1, tableErrors.Length);
        //    Assert.Equal(SchemaValidationErrorKind.Missing, tableErrors[0].Kind);

        //    var error = tableErrors[0];

        //    Assert.Equal(null, error.ObjectType);
        //    Assert.Equal(null, error.RelationType);
        //    Assert.Equal(fromC1ToC2.RoleType, error.Role);
        //    Assert.Equal("c2", error.TableName);
        //    Assert.Equal("c2c1", error.ColumnName);
        //}

        //[Fact]
        //public void ValidateNewOne2OneRelation()
        //{
        //    this.DropTable("C1");
        //    this.DropTable("C2");

        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

        //    var c1 = this.CreateClass("C1");
        //    var c2 = this.CreateClass("C2");

        //    this.CreateDatabase(this.domain.MetaPopulation, true);

        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

        //    c1 = this.CreateClass("C1");
        //    c2 = this.CreateClass("C2");

        //    // extra relation
        //    var fromC1ToC2 = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    fromC1ToC2.AssociationType.ObjectType = c1;
        //    fromC1ToC2.RoleType.ObjectType = c2;

        //    var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    var validationErrors = this.GetSchemaValidation(Database);

        //    var tableErrors = validationErrors.TableErrors;

        //    Assert.Equal(1, tableErrors.Length);
        //    Assert.Equal(SchemaValidationErrorKind.Missing, tableErrors[0].Kind);

        //    var error = tableErrors[0];

        //    Assert.Equal(null, error.ObjectType);
        //    Assert.Equal(null, error.RelationType);
        //    Assert.Equal(fromC1ToC2.RoleType, error.Role);
        //    Assert.Equal("c1", error.TableName);
        //    Assert.Equal("c2", error.ColumnName);
        //}

        //[Fact]
        //public void ValidateStringRelationDifferentSize()
        //{
        //    this.DropTable("C1");
        //    this.DropTable("C2");

        //    var environment = new MetaPopulation();
        //    var core = Repository.Core(environment);
        //    this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.AddDirectSuperdomain(core);

        //    var c1 = this.CreateClass("C1");
        //    this.CreateClass("C2");

        //    var c1RelationType = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    c1RelationType.AssociationType.ObjectType = c1;
        //    c1RelationType.RoleType.ObjectType = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.StringId);
        //    c1RelationType.RoleType.Size = 100;

        //    this.CreateDatabase(this.domain.MetaPopulation, true);

        //    environment = new MetaPopulation();
        //    core = Repository.Core(environment);
        //    this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.AddDirectSuperdomain(core);

        //    c1 = this.CreateClass("C1");
        //    this.CreateClass("C2");

        //    c1RelationType = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    c1RelationType.AssociationType.ObjectType = c1;
        //    c1RelationType.RoleType.ObjectType = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.StringId);
        //    c1RelationType.RoleType.Size = 100;

        //    // Different size
        //    c1RelationType.RoleType.Size = 101;

        //    var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    var validationErrors = this.GetSchemaValidation(Database);

        //    var tableErrors = validationErrors.TableErrors;

        //    Assert.Equal(1, tableErrors.Length);
        //    Assert.Equal(SchemaValidationErrorKind.Incompatible, tableErrors[0].Kind);

        //    var error = tableErrors[0];

        //    Assert.Equal(null, error.ObjectType);
        //    Assert.Equal(null, error.RelationType);
        //    Assert.Equal(c1RelationType.RoleType, error.Role);

        //    Assert.Equal("c1", error.TableName);
        //    Assert.Equal("allorsstring", error.ColumnName);
        //}

        //[Fact]
        //public void ValidateStringToOne2One()
        //{
        //    this.DropTable("C1");
        //    this.DropTable("C2");

        //    var environment = new MetaPopulation();
        //    var core = Repository.Core(environment);
        //    this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.AddDirectSuperdomain(core);

        //    var c1 = this.CreateClass("C1");
        //    var c2 = this.CreateClass("C2");

        //    var c1RelationType = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    c1RelationType.AssociationType.ObjectType = c1;
        //    c1RelationType.RoleType.ObjectType = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.StringId);
        //    c1RelationType.RoleType.Size = 100;
        //    c1RelationType.RoleType.SingularName = "RelationType";
        //    c1RelationType.RoleType.PluralName = "RelationTypes";

        //    this.CreateDatabase(this.domain.MetaPopulation, true);

        //    environment = new MetaPopulation();
        //    core = Repository.Core(environment);
        //    this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.AddDirectSuperdomain(core);

        //    c1 = this.CreateClass("C1");
        //    c2 = this.CreateClass("C2");

        //    c1RelationType = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    c1RelationType.AssociationType.ObjectType = c1;
        //    c1RelationType.RoleType.ObjectType = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.StringId);
        //    c1RelationType.RoleType.Size = 100;
        //    c1RelationType.RoleType.SingularName = "RelationType";
        //    c1RelationType.RoleType.PluralName = "RelationTypes";

        //    // From string to one2one
        //    c1RelationType.RoleType.Size = null;
        //    c1RelationType.RoleType.ObjectType = c2;

        //    var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    var validationErrors = this.GetSchemaValidation(Database);

        //    var tableErrors = validationErrors.TableErrors;

        //    Assert.Equal(1, tableErrors.Length);
        //    Assert.Equal(SchemaValidationErrorKind.Incompatible, tableErrors[0].Kind);

        //    var error = tableErrors[0];

        //    Assert.Equal(null, error.ObjectType);
        //    Assert.Equal(null, error.RelationType);
        //    Assert.Equal(c1RelationType.RoleType, error.Role);

        //    Assert.Equal("c1", error.TableName);
        //    Assert.Equal("relationtype", error.ColumnName);
        //}

        //[Fact]
        //public void ValidateUnitRelationDifferentType()
        //{
        //    this.DropTable("C1");
        //    this.DropTable("C2");

        //    var environment = new MetaPopulation();
        //    var core = Repository.Core(environment);
        //    this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.AddDirectSuperdomain(core);

        //    var c1 = this.CreateClass("C1");
        //    this.CreateClass("C2");

        //    var c1RelationType = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    c1RelationType.AssociationType.ObjectType = c1;
        //    c1RelationType.RoleType.ObjectType = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.BooleanId);
        //    c1RelationType.RoleType.SingularName = "RelationType";
        //    c1RelationType.RoleType.PluralName = "RelationTypes";

        //    this.CreateDatabase(this.domain.MetaPopulation, true);

        //    environment = new MetaPopulation();
        //    core = Repository.Core(environment);
        //    this.domain = new Domain(environment, Guid.NewGuid()) { Name = "MyDomain" };
        //    this.domain.AddDirectSuperdomain(core);

        //    c1 = this.CreateClass("C1");
        //    this.CreateClass("C2");

        //    c1RelationType = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    c1RelationType.AssociationType.ObjectType = c1;
        //    c1RelationType.RoleType.ObjectType = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.BooleanId);
        //    c1RelationType.RoleType.SingularName = "RelationType";
        //    c1RelationType.RoleType.PluralName = "RelationTypes";

        //    // Different type
        //    c1RelationType.RoleType.ObjectType = (ObjectType)this.domain.MetaPopulation.Find(UnitIds.Unique);

        //    var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    var validationErrors = this.GetSchemaValidation(Database);

        //    var tableErrors = validationErrors.TableErrors;

        //    Assert.Equal(1, tableErrors.Length);
        //    Assert.Equal(SchemaValidationErrorKind.Incompatible, tableErrors[0].Kind);

        //    var error = tableErrors[0];

        //    Assert.Equal(null, error.ObjectType);
        //    Assert.Equal(null, error.RelationType);
        //    Assert.Equal(c1RelationType.RoleType, error.Role);

        //    Assert.Equal("c1", error.TableName);
        //    Assert.Equal("relationtype", error.ColumnName);
        //}

        //[Fact]
        //public void IndexesMany2Many()
        //{
        //    this.CreateDatabase().Init();

        //    Assert.True(this.ExistIndex("CompanyIndexedMany2ManyPerson", "R"));
        //    Assert.False(this.ExistIndex("CompanyMany2ManyPerson", "R"));
        //}

        //[Fact]
        //public void IndexesUnits()
        //{
        //    this.CreateDatabase().Init();

        //    Assert.True(this.ExistIndex("C1", "C1AllorsInteger"));
        //    Assert.False(this.ExistIndex("C1", "C1AllorsString"));
        //}

        //[Fact]
        //public void IncompatiblePopulation()
        //{
        //    this.DropTable("C1");
        //    this.DropTable("C2");

        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

        //    var c1 = this.CreateClass("C1");
        //    var c2 = this.CreateClass("C2");

        //    this.CreateDatabase(this.domain.MetaPopulation, true);

        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

        //    c1 = this.CreateClass("C1");
        //    c2 = this.CreateClass("C2");

        //    var c1c2 = new RelationTypeBuilder(this.domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();
        //    c1c2.AssociationType.ObjectType = c1;
        //    c1c2.RoleType.ObjectType = c2;

        //    var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    ISession session = null;
        //    try
        //    {
        //        session = Database.CreateSession();
        //        Assert.True(false); // Fail
        //    }
        //    catch (SchemaValidationException e)
        //    {
        //        var validationErrors = e.ValidationErrors;
        //        Assert.True(validationErrors.HasErrors);
        //    }
        //    finally
        //    {
        //        if (session != null)
        //        {
        //            session.Rollback();
        //        }
        //    }
        //}

        //protected Class CreateClass(string name)
        //{
        //    return new ClassBuilder(this.domain, Guid.NewGuid()).WithSingularName(name).WithPluralName(name + "s").Build();
        //}

        //protected Interface CreateInterface(string name)
        //{
        //    return new InterfaceBuilder(this.domain, Guid.NewGuid()).WithSingularName(name).WithPluralName(name + "s").Build();
        //}

        protected abstract IDatabase CreateDatabase(IMetaPopulation metaPopulation, bool init);

        protected IDatabase CreateDatabase() => this.Profile.CreateDatabase();

        protected abstract void DropTable(string schema, string tableName);

        protected abstract void DropProcedure(string schema, string procedure);

        protected abstract bool ExistTable(string schema, string table);

        protected abstract int ColumnCount(string schema, string table);

        protected abstract bool ExistColumn(string schema, string table, string column, ColumnTypes columnType);

        protected abstract bool ExistPrimaryKey(string schema, string table, string column);

        protected abstract bool ExistIndex(string schema, string table, string column);

        protected abstract bool ExistProcedure(string schema, string procedure);

        protected bool CreateSessionThrowsException(IDatabase database)
        {
            try
            {
                using (database.CreateSession())
                {
                }

                return false;
            }
            catch
            {
                return true;
            }
        }

        //protected RelationType CreateDomainWithCompositeRelationType(Guid relationTypeId, Multiplicity multiplicity)
        //{
        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

        //    var c1 = new ClassBuilder(this.domain, Guid.NewGuid()).WithSingularName("C1").WithPluralName("C1s").Build();
        //    var c2 = new ClassBuilder(this.domain, Guid.NewGuid()).WithSingularName("C2").WithPluralName("C2s").Build();

        //    return new RelationTypeBuilder(this.domain, relationTypeId, Guid.NewGuid(), Guid.NewGuid())
        //        .WithMultiplicity(multiplicity)
        //        .WithObjectTypes(c1, c2)
        //        .Build();
        //}

        //protected RelationType CreateDomainWithUnitRelationType(Guid relationTypeId, Guid roleTypeObjectId)
        //{
        //    this.domain = new Domain(new MetaPopulation(), Guid.NewGuid()) { Name = "MyDomain" };

        //    var c1 = new ClassBuilder(this.domain, Guid.NewGuid()).WithSingularName("C1").WithPluralName("C1s").Build();
        //    var unitType = (ObjectType)this.domain.MetaPopulation.Find(roleTypeObjectId);

        //    return new RelationTypeBuilder(this.domain, relationTypeId, Guid.NewGuid(), Guid.NewGuid())
        //        .WithMultiplicity(Multiplicity.OneToOne)
        //        .WithObjectTypes(c1, unitType)
        //        .Build();
        //}
    }
}
