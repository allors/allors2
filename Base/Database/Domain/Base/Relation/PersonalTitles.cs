// <copyright file="PersonalTitles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    
    public partial class PersonalTitles
    {
        private static readonly Guid MisterId = new Guid("510D5267-4E69-45F7-B99E-CABAF7E42EB2");
        private static readonly Guid MissId = new Guid("93275216-70B9-4DBB-A824-AFBAC7A2B32E");

        private UniquelyIdentifiableSticky<PersonalTitle> cache;

        public PersonalTitle Mister => this.Cache[MisterId];

        public PersonalTitle Miss => this.Cache[MissId];
        
        private UniquelyIdentifiableSticky<PersonalTitle> Cache => this.cache ??= new UniquelyIdentifiableSticky<PersonalTitle>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PersonalTitleBuilder(this.Session)
                .WithName("Mister")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Mijnheer").WithLocale(dutchLocale).Build())
                .WithUniqueId(MisterId)
                .WithIsActive(true)
                .Build();

            new PersonalTitleBuilder(this.Session)
                .WithName("Miss")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Mevrouw").WithLocale(dutchLocale).Build())
                .WithUniqueId(MissId)
                .WithIsActive(true)
                .Build();
        }
    }
}
