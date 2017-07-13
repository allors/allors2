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

namespace Tests
{
    using Allors.Domain;

    using Xunit;

    
    public class MethodsTests : DomainTest
    {
        [Fact]
        public void ClassMethod()
        {
            var c1 = new C1Builder(this.Session).Build();

            var classMethod = c1.ClassMethod();

            Assert.Equal("C1CustomC1BaseC1Core", classMethod.Value);
        }

        [Fact]
        public void InterfaceMethod()
        {
            var c1 = new C1Builder(this.Session)
                .Build();

            var interfaceMethod = c1.InterfaceMethod();

            Assert.Equal("I1CustomI1BaseI1CoreC1CustomC1BaseC1Core", interfaceMethod.Value);
        }

        [Fact]
        public void SuperinterfaceMethod()
        {
            var c1 = new C1Builder(this.Session)
                .Build();

            var interfaceMethod = c1.SuperinterfaceMethod();

            Assert.Equal("S1CustomS1BaseS1CoreI1CustomI1BaseI1CoreC1CustomC1BaseC1Core", interfaceMethod.Value);
        }

        [Fact]
        public void MethodWithResults()
        {
            var c1 = new C1Builder(this.Session).Build();

            var method = c1.Sum(
                m =>
                {
                    m.a = 1;
                    m.b = 2;
                });

            Assert.Equal(3, method.result);
        }

        [Fact]
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

            Assert.True(exceptionThrown);
        }

    }
}
