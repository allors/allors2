// <copyright file="If.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Document.Xml
{
    public class If : Statement
    {
        public If(string text) => this.Text = text.Trim();

        public string Text { get; set; }
    }
}
