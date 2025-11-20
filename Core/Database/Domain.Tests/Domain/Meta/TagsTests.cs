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
    using Allors.Meta;
    using Xunit;

    public class TagsTests : DomainTest
    {
        [Fact]
        public void RelationType()
        {
            var tagged = M.Tagged;

            {
                var tags = tagged.SingleTag.RelationType.Tags;

                Assert.Single(tags);
                Assert.Equal("TagA", tags[0]);
            }

            {
                var tags = tagged.MultipleTags.RelationType.Tags;

                Assert.Equal(2, tags.Length);
                Assert.Contains("TagA", tags);
                Assert.Contains("TagB", tags);
            }

        }

        [Fact]
        public void MethodType()
        {
            var tagged = M.Tagged;

            {
                var tags = tagged.SingleTagMethod.Tags;

                Assert.Single(tags);
                Assert.Equal("TagX", tags[0]);
            }

            {
                var tags = tagged.MultipleTagMethod.Tags;

                Assert.Equal(2, tags.Length);
                Assert.Contains("TagX", tags);
                Assert.Contains("TagY", tags);
            }
        }

    }
}
