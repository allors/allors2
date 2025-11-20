// <copyright file="LocalisedText.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Meta;

    public partial class LocalisedTextAccessor
    {
        private readonly RelationType relationType;

        public LocalisedTextAccessor(RoleType roleType) => this.relationType = roleType.RelationType;

        public string Get(IObject @object, Locale locale)
        {
            var localisedTexts = @object.Strategy.GetCompositeRoles(this.relationType);
            foreach (LocalisedText localisedText in localisedTexts)
            {
                if (localisedText?.Locale?.Equals(locale) == true)
                {
                    return localisedText.Text;
                }
            }

            return null;
        }

        public void Set(IObject @object, Locale locale, string text)
        {
            var localisedTexts = @object.Strategy.GetCompositeRoles(this.relationType);
            foreach (LocalisedText existingLocalisedText in localisedTexts)
            {
                if (existingLocalisedText?.Locale?.Equals(locale) == true)
                {
                    existingLocalisedText.Text = text;
                    return;
                }
            }

            var newLocalisedText = new LocalisedTextBuilder(@object.Strategy.Session)
                .WithLocale(locale)
                .WithText(text)
                .Build();
            @object.Strategy.AddCompositeRole(this.relationType, newLocalisedText);
        }
    }
}
