// <copyright file="Class.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Typescript
{
    using System;
    using Autotest.Angular;
    using Newtonsoft.Json.Linq;

    public class Class
    {
        private Directive directive;
        private JToken typeTemplate;

        public Class(Directive directive, JToken typeTemplate)
        {
            this.directive = directive;
            this.typeTemplate = typeTemplate;
        }

        public string Name { get; set; }

        public Decorator[] Decorators { get; set; }

        private IMember[] Members { get; set; }

        internal void BaseLoad()
        {
        }
    }
}