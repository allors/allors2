// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateException.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba
// This file is licenses under the Lesser General Public Licence v3 (LGPL)
// The LGPL License is included in the file LICENSE.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Document
{
    using System;
    using System.Collections.ObjectModel;

    using Antlr4.StringTemplate.Misc;

    public class TemplateException : Exception
    {
        public readonly ReadOnlyCollection<TemplateMessage> TemplateMessages;

        internal TemplateException(ReadOnlyCollection<TemplateMessage> errorBufferErrors)
            : base(string.Join("\n", string.Join("\n", errorBufferErrors)))
        {
            this.TemplateMessages = errorBufferErrors;
        }
    }
}
