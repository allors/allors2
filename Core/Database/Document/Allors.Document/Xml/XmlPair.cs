// <copyright file="XmlPair.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Document.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class XmlPair
    {
        private XmlElement end;

        public XmlPair(XmlPair parent, XmlElement begin)
        {
            this.Parent = parent;
            this.Children = new List<XmlPair>();
            this.Begin = begin;
        }

        public XmlPair Parent { get; }

        public XmlDocument Document => this.Begin.OwnerDocument;

        public IList<XmlPair> Children { get; }

        public XmlElement Begin { get; }

        public XmlElement End
        {
            get => this.end;
            set
            {
                this.end = value;
                this.FindSiblings();
            }
        }

        public XmlElement BeginSibling { get; private set; }

        public XmlElement EndSibling { get; private set; }

        public bool IsRoot => this.Parent == null;

        public void Rewrite(Func<XmlPair, XmlElement> factory)
        {
            if (!this.IsRoot)
            {
                var parentNode = this.BeginSibling.ParentNode;
                var document = parentNode.OwnerDocument;

                var instructionElement = factory(this);

                var nextSibling = this.BeginSibling.NextSibling;
                while (nextSibling != null && nextSibling != this.EndSibling)
                {
                    instructionElement.AppendChild(nextSibling);
                    nextSibling = this.BeginSibling.NextSibling;
                }

                parentNode.InsertBefore(instructionElement, this.BeginSibling);

                parentNode.RemoveChild(this.BeginSibling);
                parentNode.RemoveChild(this.EndSibling);
            }

            foreach (var child in this.Children)
            {
                child.Rewrite(factory);
            }
        }

        public override string ToString() => this.Begin.LocalName + ": " + this.Begin.InnerText;

        private void FindSiblings()
        {
            var beginAncestry = new List<XmlElement> { this.Begin };
            this.GetAncestry(this.Begin, beginAncestry);

            var endAncestry = new List<XmlElement> { this.End };
            this.GetAncestry(this.End, endAncestry);

            XmlElement lowestCommonAncestor = null;
            foreach (var ancestor in beginAncestry)
            {
                if (endAncestry.Contains(ancestor))
                {
                    lowestCommonAncestor = ancestor;
                    break;
                }
            }

            if (lowestCommonAncestor != null)
            {
                this.BeginSibling = beginAncestry[beginAncestry.IndexOf(lowestCommonAncestor) - 1];
                this.EndSibling = endAncestry[endAncestry.IndexOf(lowestCommonAncestor) - 1];
            }
        }

        private void GetAncestry(XmlElement element, List<XmlElement> ancestry)
        {
            while (true)
            {
                if (element != null)
                {
                    ancestry.Add(element);
                    if (element.ParentNode is XmlElement node)
                    {
                        element = node;
                    }
                    else
                    {
                        element = null;
                    }

                    continue;
                }

                break;
            }
        }
    }
}
