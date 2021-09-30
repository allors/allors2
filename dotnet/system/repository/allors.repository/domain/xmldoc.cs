// <copyright file="XmlDoc.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public class XmlDoc
    {
        public XmlDoc(string value)
        {
            this.FullValue = value;

            try
            {
                var element = XElement.Parse(value);
                this.Value = element.Elements().First().ToString();
            }
            catch
            {
                throw new Exception("Could not parse XmlDoc: \n" + this.FullValue);
            }
        }

        public string FullValue { get; }

        public string VerbatimFullValue => this.FullValue?.Replace("\"", "\"\"");

        public string Value { get; }

        public string VerbatimValue => this.Value?.Replace("\"", "\"\"");
    }
}
