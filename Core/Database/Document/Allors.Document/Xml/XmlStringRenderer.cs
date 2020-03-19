// <copyright file="Binding.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Globalization;
using System.Text;
using Antlr4.StringTemplate;

namespace Allors.Document.Xml
{
    public class XmlStringRenderer : IAttributeRenderer
    {
        private static readonly StringRenderer DefaultRenderer = new StringRenderer();

        public string ToString(object obj, string formatString, CultureInfo culture)
        {
            if ("xml-string".Equals(formatString))
            {
                return XmlString(obj as string);
            }

            return DefaultRenderer.ToString(obj, formatString, culture);
        }

        public static string XmlString(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                var stringBuilder = new StringBuilder(text.Length);

                foreach (var chr in text)
                {
                    if (chr == '<')
                    {
                        stringBuilder.Append("&lt;");
                    }
                    else if (chr == '>')
                    {
                        stringBuilder.Append("&gt;");
                    }
                    else if (chr == '&')
                    {
                        stringBuilder.Append("&amp;");
                    }
                    else if (chr >= 32)
                    {
                        stringBuilder.Append(chr);
                    }
                }

                return stringBuilder.ToString();
            }

            return string.Empty;
        }
    }
}
