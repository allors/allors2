// <copyright file="IPropertyTypesExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Workspace.Data
{
    using Allors.Workspace.Meta;

    public static partial class IPropertyTypesExtensions
    {
        public static Node Path(this IPropertyType[] @this)
        {
            if (@this == null)
            {
                return null;
            }

            Node node = null;
            Node currentNode = null;

            foreach (var propertyType in @this)
            {
                if (node == null)
                {
                    node = new Node(propertyType);
                    currentNode = node;
                }
                else
                {
                    var newNode = new Node(propertyType);
                    currentNode.Add(newNode);
                    currentNode = newNode;
                }
            }

            return node;
        }
    }
}
