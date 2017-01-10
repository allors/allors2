// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Population.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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

namespace Allors.Meta.Static
{
    using System;
    using Allors.Meta;

    public class Population
    {
        private readonly MetaPopulation metaPopulation;

        public Population()
        {
            this.metaPopulation = new MetaPopulation();

            var allors = Repository.TestOnPostInit(this.metaPopulation);

            var validationlog = this.metaPopulation.Validate();

            this.Domain = new Domain(this.MetaPopulation, Guid.NewGuid()) { Name = "Domain" };
            this.Domain.AddDirectSuperdomain(allors);

            this.GetUnits();
        }

        public Unit BinaryType { get; set; }

        public Unit BooleanType { get; set; }

        public Unit DecimalType { get; set; }

        public Unit DoubleType { get; set; }

        public Unit IntegerType { get; set; }

        public Unit LongType { get; set; }

        public Unit StringType { get; set; }

        public Unit UniqueType { get; set; }

        public Interface I1 { get; set; }

        public Interface I12 { get; set; }

        public Interface I2 { get; set; }

        public Interface I3 { get; set; }

        public Interface I34 { get; set; }

        public Interface I4 { get; set; }

        public Class C1 { get; set; }

        public Class C2 { get; set; }

        public Class C3 { get; set; }

        public Class C4 { get; set; }

        public Domain Domain { get; set; }

        public Domain SuperDomain { get; set; }

        public MetaPopulation MetaPopulation
        {
            get
            {
                return this.metaPopulation;
            }
        }

        public static Class CreateClass(Domain domain, string name)
        {
            return new ClassBuilder(domain, Guid.NewGuid()).WithSingularName(name).WithPluralName(name + "s").Build();
        }

        public static Interface CreateInterface(Domain domain, string name)
        {
            return new InterfaceBuilder(domain, Guid.NewGuid()).WithSingularName(name).WithPluralName(name + "s").Build();
        }
        
        internal void Populate()
        {
            this.PopulateSuperDomain();

            // interfaces
            this.I12 = CreateInterface(this.Domain, "i12");
            this.I34 = CreateInterface(this.Domain, "i34");
            this.I1 = CreateInterface(this.Domain, "i1");
            this.I2 = CreateInterface(this.Domain, "i2");
            this.I3 = CreateInterface(this.Domain, "i3");
            this.I4 = CreateInterface(this.Domain, "i4");

            // classes
            this.C1 = CreateClass(this.Domain, "c1");
            this.C2 = CreateClass(this.Domain, "c2");
            this.C3 = CreateClass(this.Domain, "c3");
            this.C4 = CreateClass(this.Domain, "c4");

            this.CreateInheritance();

            var validation = this.MetaPopulation.Validate();
            if (validation.ContainsErrors)
            {
                throw new Exception("Domain invalid");
            }
        }

        internal void PopulateWithSuperDomains()
        {
            this.PopulateSuperDomain();

            // interfaces
            this.I12 = CreateInterface(this.SuperDomain, "i12");
            this.I34 = CreateInterface(this.Domain, "i34");
            this.I1 = CreateInterface(this.SuperDomain, "i1");
            this.I2 = CreateInterface(this.SuperDomain, "i2");
            this.I3 = CreateInterface(this.Domain, "i3");
            this.I4 = CreateInterface(this.Domain, "i4");

            // classes
            this.C1 = CreateClass(this.Domain, "c1");
            this.C2 = CreateClass(this.Domain, "c2");
            this.C3 = CreateClass(this.Domain, "c3");
            this.C4 = CreateClass(this.Domain, "c4");

            this.CreateInheritance();

            var validation = this.MetaPopulation.Validate();
            if (validation.ContainsErrors)
            {
                throw new Exception("Domain invalid");
            }
        }

        private void PopulateSuperDomain()
        {
            this.SuperDomain = new Domain(this.MetaPopulation, Guid.NewGuid()) { Name = "SuperDomain" };
            this.Domain.AddDirectSuperdomain(this.SuperDomain);
        }

        private void CreateInheritance()
        {
            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(this.C1).WithSupertype(this.I1).Build();
            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(this.C1).WithSupertype(this.I12).Build();

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(this.C2).WithSupertype(this.I2).Build();
            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(this.C2).WithSupertype(this.I12).Build();

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(this.C3).WithSupertype(this.I3).Build();
            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(this.C3).WithSupertype(this.I34).Build();

            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(this.C4).WithSupertype(this.I4).Build();
            new InheritanceBuilder(this.Domain, Guid.NewGuid()).WithSubtype(this.C4).WithSupertype(this.I34).Build();
        }
        
        private void GetUnits()
        {
            this.BinaryType = (Unit)this.metaPopulation.Find(UnitIds.BinaryId);
            this.BooleanType = (Unit)this.metaPopulation.Find(UnitIds.BooleanId);
            this.DecimalType = (Unit)this.metaPopulation.Find(UnitIds.DecimalId);
            this.DoubleType = (Unit)this.metaPopulation.Find(UnitIds.FloatId);
            this.IntegerType = (Unit)this.metaPopulation.Find(UnitIds.IntegerId);
            this.StringType = (Unit)this.metaPopulation.Find(UnitIds.StringId);
            this.UniqueType = (Unit)this.metaPopulation.Find(UnitIds.Unique);
        }
    }
}