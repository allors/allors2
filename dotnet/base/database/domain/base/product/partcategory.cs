// <copyright file="PartCategory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    public partial class PartCategory
    {
        public override string ToString() => this.Name;

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                iteration.AddDependency(this.PrimaryParent, this);
                iteration.Mark(this.PrimaryParent);

                foreach (Object parent in this.SecondaryParents)
                {
                    iteration.AddDependency(parent, this);
                    iteration.Mark(parent);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var defaultLocale = this.Strategy.Session.GetSingleton().DefaultLocale;

            if (this.LocalisedNames.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Name = this.LocalisedNames.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            if (this.LocalisedDescriptions.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Description = this.LocalisedDescriptions.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            if (!this.ExistCategoryImage)
            {
                this.CategoryImage = this.Strategy.Session.GetSingleton().Settings.NoImageAvailableImage;
            }

            {
                var primaryAncestors = new List<PartCategory>();
                var primaryAncestor = this.PrimaryParent;
                while (primaryAncestor != null)
                {
                    if (primaryAncestors.Contains(primaryAncestor))
                    {
                        var cycle = string.Join(" -> ", primaryAncestors.Append(primaryAncestor).Select(v => v.Name));
                        derivation.Validation.AddError(this, this.Meta.PrimaryParent, "Cycle detected in " + cycle);
                        break;
                    }

                    primaryAncestors.Add(primaryAncestor);
                    primaryAncestor = primaryAncestor.PrimaryParent;
                }

                this.PrimaryAncestors = primaryAncestors.ToArray();
            }

            this.Children = this.PartCategoriesWherePrimaryParent.Union(this.PartCategoriesWhereSecondaryParent).ToArray();
            {
                var descendants = new List<PartCategory>();
                var children = this.Children.ToArray();
                while (children.Length > 0)
                {
                    if (children.Any(v => descendants.Contains(v)))
                    {
                        var cycle = string.Join(" -> ", descendants.Union(children).Select(v => v.Name));
                        derivation.Validation.AddError(this, this.Meta.Children, "Cycle detected in " + cycle);
                        break;
                    }

                    descendants.AddRange(children);
                    children = children.SelectMany(v => v.Children).ToArray();
                }

                this.Descendants = descendants.ToArray();
            }

            var descendantsAndSelf = this.Descendants.Append(this).ToArray();
        }
    }
}
