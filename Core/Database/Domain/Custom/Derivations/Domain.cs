// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;

    public static partial class DabaseExtensions
    {
        public class FullNameDerivation : IDomainDerivation
        {
            public void Derive(IDomainChangeSet changeSet)
            {
                changeSet.AssociationsByRoleType.TryGetValue(M.Person.FirstName, out var firstNames);
                var personFirstNames = firstNames?.OfType<Person>();

                changeSet.AssociationsByRoleType.TryGetValue(M.Person.LastName, out var lastNames);
                var personLastNames = lastNames?.OfType<Person>();

                if (personFirstNames?.Any() == true || lastNames?.Any() == true)
                {
                    var people = personFirstNames.Union(personLastNames).Distinct();

                    foreach (var person in people)
                    {
                        person.DomainFullName = $"{person.FirstName} {person.LastName}";
                    }
                }
            }
        }

        public class GreetingDerivation : IDomainDerivation
        {
            public void Derive(IDomainChangeSet changeSet)
            {
                changeSet.AssociationsByRoleType.TryGetValue(M.Person.FullName, out var fullNames);
                var personFullNames = fullNames?.OfType<Person>();

                if (personFullNames?.Any() == true)
                {
                    var people = personFullNames;

                    foreach (var person in people)
                    {
                        person.DomainGreeting = $"Hello {person.FullName}!";
                    }
                }
            }
        }

        public static void CoreRegisterDerivations(this IDatabase @this)
        {
            @this.DomainDerivationById[new System.Guid("A4E2008B-BFAB-43EF-9090-E2F90ADFBBF1")] = new FullNameDerivation();
            @this.DomainDerivationById[new System.Guid("A5C6A30E-FB67-4D7E-89A8-68C91DA89DC2")] = new GreetingDerivation();
        }
    }
}
