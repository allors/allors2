// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodsTests.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Domain
{
    using Allors.Domain;

    using NUnit.Framework;

    [TestFixture]
    public class MethodsTests : DomainTest
    {
        [Test]
        public void ClassMethod()
        {
            var c1 = new C1Builder(this.Session).Build();

            var classMethod = c1.ClassMethod();

            Assert.AreEqual("C1CustomC1BaseC1Core", classMethod.Value);
        }

        [Test]
        public void InterfaceMethod()
        {
            var c1 = new C1Builder(this.Session)
                .Build();

            var interfaceMethod = c1.InterfaceMethod();

            Assert.AreEqual("I1CustomI1BaseI1CoreC1CustomC1BaseC1Core", interfaceMethod.Value);
        }

        [Test]
        public void SuperinterfaceMethod()
        {
            var c1 = new C1Builder(this.Session)
                .Build();

            var interfaceMethod = c1.SuperinterfaceMethod();

            Assert.AreEqual("S1CustomS1BaseS1CoreI1CustomI1BaseI1CoreC1CustomC1BaseC1Core", interfaceMethod.Value);
        }

        [Test]
        public void MethodWithResults()
        {
            var c1 = new C1Builder(this.Session).Build();

            var method = c1.Sum(
                m =>
                {
                    m.a = 1;
                    m.b = 2;
                });

            Assert.AreEqual(3, method.result);
        }

        [Test]
        public void CallMethodTwice()
        {
            var c1 = new C1Builder(this.Session)
                .Build();

            var classMethod = c1.ClassMethod();

            var exceptionThrown = false;
            try
            {
                classMethod.Execute();
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

    }
}
