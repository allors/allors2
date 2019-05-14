// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentTest.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.General
{
    using System.Collections.Generic;

    using Allors;
    using Allors.Meta;

    using Xunit;

    public abstract class ExtentTest : Test
    {
        [Fact]
        [Trait("Category", "Dynamic")]
        public void ObjectTypes()
        {
            var objectsByObjectType = new Dictionary<ObjectType, List<IObject>>();

            const int Max = 1;
            foreach (var concreteCompositeType in GetMetaDomain().ConcreteCompositeObjectTypes)
            {
                var objects = new List<IObject>();
                for (var i = 0; i < Max; i++)
                {
                    objects.Add(this.GetSession().Create(concreteCompositeType));
                }

                objectsByObjectType[concreteCompositeType] = objects;
            }

            foreach (var objectType in GetMetaDomain().CompositeObjectTypes)
            {
                if (!objectType.IsConcreteComposite)
                {
                    var objects = new List<IObject>();

                    foreach (var concreteCompositeType in objectType.ConcreteClasses)
                    {
                        objects.AddRange(objectsByObjectType[concreteCompositeType]);   
                    }
                    
                    objectsByObjectType[objectType] = objects;
                }
            }

            foreach (var objectType in GetMetaDomain().CompositeObjectTypes)
            {
                if (objectType.ConcreteClasses.Length > 0)
                {
                    object[] extent = this.GetSession().Extent(objectType);
                    var objects = objectsByObjectType[objectType];

                    Assert.Equal(objects.Count, extent.Length);
                    foreach (object extentObject in extent)
                    {
                        Assert.Contains(extentObject, objects);
                    }
                }
            }
        }

        [Fact]
        [Trait("Category", "Dynamic")]
        public void AddEquals()
        {
            var concreteClasses = this.GetTestTypes();
            foreach (var concreteClass in concreteClasses)
            {
                var extent = this.GetSession().Extent(concreteClass);

                foreach (var role in concreteClass.RoleTypes)
                {
                    if (role.ObjectType.IsUnit)
                    {
                        //TODO:
                    }
                    else if (role.IsOne)
                    {
                        foreach (var concreteType in role.ObjectType.ConcreteClasses)
                        {
                            var roleObject = this.GetSession().Create(concreteType);
                            extent.Filter.AddEquals(role, roleObject);
                        }
                    }
                }

                foreach (var association in concreteClass.AssociationTypes)
                {
                    if (association.IsOne)
                    {
                        foreach (var concreteType in association.ObjectType.ConcreteClasses)
                        {
                            var associationObject = this.GetSession().Create(concreteType);
                            extent.Filter.AddEquals(association, associationObject);
                        }
                    }
                }

                int count = extent.Count;
            }
        }

        [Fact]
        [Trait("Category", "Dynamic")]
        public void AddExist()
        {
            var concreteClasses = this.GetTestTypes();
            foreach (var concreteClass in concreteClasses)
            {
                foreach (var role in concreteClass.RoleTypes)
                {
                    var extent = this.GetSession().Extent(concreteClass);
                    extent.Filter.AddExists(role);

                    foreach (var association in concreteClass.AssociationTypes)
                    {
                        extent.Filter.AddExists(association);
                    }

                    int count = extent.Count;
                }
            }
        }

        private ObjectType[] GetTestTypes()
        {
            return GetMetaDomain().ConcreteCompositeObjectTypes;
        }
    }
}