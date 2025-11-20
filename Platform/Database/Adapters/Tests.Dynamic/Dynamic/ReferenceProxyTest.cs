// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceProxyTest.cs" company="Allors bv">
//   Copyright Allors bv.
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
    using Allors;
    using Adapters;
    using Allors.Meta;

    public abstract class ReferenceProxyTest : ReferenceTest
    {
        private readonly ReferenceTest subject;

        protected ReferenceProxyTest(ReferenceTest referenceTest)
        {
            this.subject = referenceTest;
        }

        public override void Dispose() => this.subject.Dispose();

        public override IObject[] CreateArray(ObjectType objectType, int count)
        {
            return this.subject.CreateArray(objectType, count);
        }

        public override IDatabase CreateMemoryPopulation()
        {
            return this.subject.CreateMemoryPopulation();
        }

        public override int[] GetAssertRepeats()
        {
            return this.subject.GetAssertRepeats();
        }

        public override int GetAssociationCount()
        {
            return this.subject.GetAssociationCount();
        }

        public override bool[] GetBooleanFlags()
        {
            return this.subject.GetBooleanFlags();
        }

        public override MetaPopulation GetMetaPopulation()
        {
            return this.subject.GetMetaPopulation();
        }

        public override MetaPopulation GetMetaPopulation2()
        {
            return this.subject.GetMetaPopulation2();
        }

        public override IClass GetMetaType(IObject allorsObject)
        {
            return this.subject.GetMetaType(allorsObject);
        }

        public override IDatabase GetPopulation()
        {
            return this.subject.GetPopulation();
        }

        public override IDatabase GetPopulation2()
        {
            return this.subject.GetPopulation2();
        }

        public override int[] GetRepeats()
        {
            return this.subject.GetRepeats();
        }

        public override int GetRoleCount()
        {
            return this.subject.GetRoleCount();
        }

        public override int GetRoleGroupCount()
        {
            return this.subject.GetRoleGroupCount();
        }

        public override int GetRolesPerGroup()
        {
            return this.subject.GetRolesPerGroup();
        }

        public override ISession GetSession()
        {
            return this.subject.GetSession();
        }

        public override ISession GetSession2()
        {
            return this.subject.GetSession2();
        }

        public override int[] GetTestRepeats()
        {
            return this.subject.GetTestRepeats();
        }

        public override bool IsRollbackSupported()
        {
            return this.subject.IsRollbackSupported();
        }
    }
}
