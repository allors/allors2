// <copyright file="TesterFactory.v.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Testers
{
    using Autotest.Html;

    public static partial class TesterFactory
    {
        public static Tester Create(Element element) => CustomCreate(element) ?? BaseCreate(element);
    }
}
