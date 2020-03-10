// <copyright file="OpenDocumentTemplateContent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Document.OpenDocument
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml;

    using Allors.Document.Xml;

    public class OpenDocumentTemplateContent
    {
        private static readonly Regex BindingRegex = new Regex(@"<\$(.*)>", RegexOptions.IgnoreCase);
        private static readonly Regex ForRegex = new Regex(@"<@for(.*)>", RegexOptions.IgnoreCase);
        private static readonly Regex IfRegex = new Regex(@"<@if(.*)>", RegexOptions.IgnoreCase);
        private static readonly Regex EndRegex = new Regex(@"<@end(.*)>", RegexOptions.IgnoreCase);

        private readonly XmlDocument template;

        private readonly Dictionary<XmlElement, Binding> bindingByXmlElement;
        private readonly Dictionary<XmlElement, Statement> statementByXmlElement;

        public OpenDocumentTemplateContent(byte[] content, char leftDelimiter, char rightDelimiter)
        {
            this.LeftDelimiter = leftDelimiter;
            this.RightDelimiter = rightDelimiter;

            this.bindingByXmlElement = new Dictionary<XmlElement, Binding>();
            this.statementByXmlElement = new Dictionary<XmlElement, Statement>();

            this.template = new XmlDocument();
            using (var input = new MemoryStream(content))
            {
                this.template.Load(input);
            }

            var root = new XmlPair(null, null);
            var stack = new Stack<XmlPair>();
            var documentElement = this.template.DocumentElement;

            var walker = root;
            var bindingByOriginalXmlElement = new Dictionary<XmlElement, Binding>();
            this.Prepare(documentElement, ref walker, stack, bindingByOriginalXmlElement);

            var forCounter = 0;
            XmlElement Factory(XmlPair pair)
            {
                var text = pair.Begin.InnerText.Trim();
                var match = ForRegex.Match(text);
                if (match.Success)
                {
                    var element = pair.Document.CreateElement("for");
                    var statement = new For(++forCounter, match.Groups[1].ToString());
                    this.statementByXmlElement.Add(element, statement);
                    return element;
                }

                if ((match = IfRegex.Match(text)).Success)
                {
                    var element = pair.Document.CreateElement("if");
                    var statement = new If(match.Groups[1].ToString());
                    this.statementByXmlElement.Add(element, statement);
                    return element;
                }

                throw new Exception("Unknown statement: " + pair.Begin.InnerText);
            }

            foreach (var pair in bindingByOriginalXmlElement)
            {
                var xmlElement = pair.Key;
                var binding = pair.Value;

                var bindingElement = this.template.CreateElement("binding");
                var parent = xmlElement.ParentNode;
                parent.InsertBefore(bindingElement, xmlElement);
                parent.RemoveChild(xmlElement);

                this.bindingByXmlElement.Add(bindingElement, binding);
            }

            root.Rewrite(Factory);
        }

        private char LeftDelimiter { get; }

        private char RightDelimiter { get; }

        public string ToStringTemplate()
        {
            using (var writer = new StringWriter())
            {
                writer.Write(@" ::= <<
<?xml version=""1.0"" encoding=""UTF-8""?>
");
                this.Write(this.template.DocumentElement, writer);

                writer.Write(@"
>>");

                foreach (var keyPair in this.statementByXmlElement)
                {
                    var element = keyPair.Key;
                    var statement = keyPair.Value;

                    if (statement is For forStatement)
                    {
                        writer.Write(@"

");
                        writer.Write(forStatement.Statement);
                        writer.Write(@" ::= <<");

                        this.Write(element.ChildNodes, writer);

                        writer.Write(@"
>>");
                    }
                }

                return writer.ToString();
            }
        }

        private void Prepare(XmlElement parent, ref XmlPair parentPair, Stack<XmlPair> stack, Dictionary<XmlElement, Binding> bindingByOriginalXmlElement)
        {
            foreach (XmlNode node in parent.ChildNodes)
            {
                if (node is XmlElement element)
                {
                    if (element.LocalName.Equals("placeholder") && element.NamespaceURI.Contains("opendocument:xmlns:text"))
                    {
                        var text = element.InnerText.Trim();

                        // Bindings
                        var match = BindingRegex.Match(text);
                        if (match.Success)
                        {
                            bindingByOriginalXmlElement[element] = new Binding(match.Groups[1].ToString());
                        }

                        // Instructions (for/if)
                        if (ForRegex.Match(text).Success || IfRegex.Match(text).Success)
                        {
                            var childNode = new XmlPair(parentPair, element);
                            stack.Push(childNode);
                            parentPair.Children.Add(childNode);
                            parentPair = childNode;
                        }

                        // Instructions (end)
                        if (EndRegex.Match(text).Success)
                        {
                            var childNode = stack.Pop();
                            childNode.End = element;
                            parentPair = childNode.Parent;
                        }
                    }

                    if (element.LocalName.Equals("frame") && element.NamespaceURI.Contains("opendocument:xmlns:drawing"))
                    {
                        var drawNs = element.GetNamespaceOfPrefix("draw");
                        var name = element.GetAttribute("name", drawNs).Trim();
                        if (name.StartsWith("$"))
                        {
                            var value = this.LeftDelimiter + name.Substring(1) + this.RightDelimiter;
                            element.SetAttribute("name", drawNs, value);
                        }
                    }

                    this.Prepare(element, ref parentPair, stack, bindingByOriginalXmlElement);
                }
            }
        }

        private void Write(XmlElement element, TextWriter writer)
        {
            if (this.bindingByXmlElement.TryGetValue(element, out var binding))
            {
                writer.Write(this.LeftDelimiter);
                writer.Write(binding.Text);
                writer.Write(@"; format=""xml-string""");
                writer.Write(this.RightDelimiter);
                return;
            }

            if (this.statementByXmlElement.TryGetValue(element, out var statement))
            {
                if (statement is If ifStatement)
                {
                    writer.Write(this.LeftDelimiter);
                    writer.Write("if(");
                    writer.Write(ifStatement.Text);
                    writer.Write(")");
                    writer.Write(this.RightDelimiter);

                    this.Write(element.ChildNodes, writer);

                    writer.Write(@"
");
                    writer.Write(this.LeftDelimiter);
                    writer.Write("endif");
                    writer.Write(this.RightDelimiter);
                }
                else if (statement is For forStatement)
                {
                    writer.Write(this.LeftDelimiter);
                    writer.Write(forStatement.Expression);
                    writer.Write(":{");
                    writer.Write(forStatement.Argument);
                    writer.Write("|");
                    writer.Write(this.LeftDelimiter);
                    writer.Write(forStatement.Statement);
                    writer.Write(this.RightDelimiter);
                    writer.Write("}");
                    writer.Write(this.RightDelimiter);
                }
                else
                {
                    throw new Exception("Unknown statement: " + statement.GetType() + " - " + statement);
                }

                return;
            }

            writer.Write("<");
            if (!string.IsNullOrWhiteSpace(element.Prefix))
            {
                writer.Write(element.Prefix);
                writer.Write(":");
            }

            writer.Write(element.LocalName);

            foreach (XmlAttribute attribute in element.Attributes)
            {
                writer.Write(" ");
                if (!string.IsNullOrWhiteSpace(attribute.Prefix))
                {
                    writer.Write(attribute.Prefix);
                    writer.Write(":");
                }

                writer.Write(attribute.LocalName);
                writer.Write(@"=""");
                writer.Write(attribute.Value);
                writer.Write(@"""");
            }

            writer.Write(">");

            this.Write(element.ChildNodes, writer);

            writer.Write(@"
</");
            if (!string.IsNullOrWhiteSpace(element.Prefix))
            {
                writer.Write(element.Prefix);
                writer.Write(":");
            }

            writer.Write(element.LocalName);
            writer.Write(">");
        }

        private void Write(XmlNodeList childNodes, TextWriter writer)
        {
            foreach (XmlNode childNode in childNodes)
            {
                switch (childNode.NodeType)
                {
                    case XmlNodeType.Element:
                        writer.Write(@"
");
                        this.Write((XmlElement)childNode, writer);
                        break;

                    case XmlNodeType.Text:
                        writer.Write(childNode.Value);
                        break;

                    // TODO: Other XmlNodeTypes

                    // Ignore the following XmlNodeTypes
                    case XmlNodeType.EndElement:
                        break;
                }
            }
        }
    }
}
