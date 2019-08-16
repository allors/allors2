// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Binding.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba
// This file is licenses under the Lesser General Public Licence v3 (LGPL)
// The LGPL License is included in the file LICENSE.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Document.Xml
{
    public class Binding
    {
        public string text;

        public Binding(string text) => this.text = text.Trim();
    }
}
