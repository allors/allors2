// <copyright file="MethodsTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

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

            Assert.Equal("C1CustomC1Core", classMethod.Value);
        }

        [Fact]
        public void InterfaceMethod()
        {
            var c1 = new C1Builder(this.Session)
                .Build();

            var interfaceMethod = c1.InterfaceMethod();

            Assert.Equal("I1CustomI1CoreC1CustomC1Core", interfaceMethod.Value);
        }

        [Fact]
        public void SuperinterfaceMethod()
        {
            var c1 = new C1Builder(this.Session)
                .Build();

            var interfaceMethod = c1.SuperinterfaceMethod();

            Assert.Equal("S1CustomS1CoreI1CustomI1CoreC1CustomC1Core", interfaceMethod.Value);
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
