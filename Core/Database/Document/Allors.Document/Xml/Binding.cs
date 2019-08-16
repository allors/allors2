
// <copyright file="Binding.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Document.Xml
{
    public class Binding
    {
        public string text;

        public Binding(string text) => this.text = text.Trim();
    }
}
