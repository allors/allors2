// <copyright file="TemplateException.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Document
{
    using System;
    using System.Collections.ObjectModel;

    using Antlr4.StringTemplate.Misc;

#pragma warning disable RCS1194 // Implement exception constructors.

    public class TemplateException : Exception
    {
        public readonly ReadOnlyCollection<TemplateMessage> TemplateMessages;

        internal TemplateException(ReadOnlyCollection<TemplateMessage> errorBufferErrors)
            : base(string.Join("\n", string.Join("\n", errorBufferErrors))) =>
            this.TemplateMessages = errorBufferErrors;
    }
}
