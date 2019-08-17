// <copyright file="XmlPairTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allos.Document.Xml.Tests
{
    using System.Xml;
    using Allors.Document.Xml;
    using Org.XmlUnit.Builder;
    using Xunit;

    public class XmlPairTests
    {
        [Fact]
        public void RewriteSiblingParents()
        {
            var xml = @"
<root>
  <parent1/>
  <parent2/>
  <parent3/>
</root>
";

            var document = new XmlDocument();
            document.LoadXml(xml);

            var parent1 = (XmlElement)document.GetElementsByTagName("parent1")[0];
            var parent3 = (XmlElement)document.GetElementsByTagName("parent3")[0];

            var rootPair = new XmlPair(null, null);
            var pair = new XmlPair(rootPair, parent1) { End = parent3 };
            rootPair.Children.Add(pair);

            rootPair.Rewrite(v => v.Document.CreateElement("allors"));

            var expected = @"
<root>
  <allors>
    <parent2/>
  </allors>
</root>";

            var diff = DiffBuilder.Compare(Input.FromDocument(document)).WithTest(Input.FromString(expected)).Build();

            Assert.False(diff.HasDifferences(), diff.ToString());
        }

        [Fact]
        public void RewriteNestedSiblingParents()
        {
            var xml = @"
<root>
  <parent1/>
  <parent2/>
  <parent3/>
  <parent4/>
  <parent5/>
</root>
";

            var document = new XmlDocument();
            document.LoadXml(xml);

            var rootPair = new XmlPair(null, null);

            var parent1 = (XmlElement)document.GetElementsByTagName("parent1")[0];
            var parent5 = (XmlElement)document.GetElementsByTagName("parent5")[0];
            var pair1 = new XmlPair(rootPair, parent1) { End = parent5 };
            rootPair.Children.Add(pair1);

            var parent2 = (XmlElement)document.GetElementsByTagName("parent2")[0];
            var parent4 = (XmlElement)document.GetElementsByTagName("parent4")[0];
            var pair2 = new XmlPair(rootPair, parent2) { End = parent4 };
            pair1.Children.Add(pair2);

            rootPair.Rewrite(v => v.Document.CreateElement("allors"));

            var expected = @"
<root>
  <allors>
    <allors>
       <parent3/>
    </allors>
  </allors>
</root>";

            var diff = DiffBuilder.Compare(Input.FromDocument(document)).WithTest(Input.FromString(expected)).Build();
            Assert.False(diff.HasDifferences(), diff.ToString());
        }
    }
}
