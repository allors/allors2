import { MetaPopulation } from '@allors/meta/system';
import { And, Like } from '@allors/data/system';

import { FilterDefinition } from '@allors/angular/core';

import { Meta } from '@allors/meta/generated';

export function configure(metaPopulation: MetaPopulation) {
  const m = metaPopulation as Meta;

  m.Person.filterDefinition = new FilterDefinition(
    new And([
      new Like({ roleType: m.Person.FirstName, parameter: 'firstName' }),
      new Like({ roleType: m.Person.LastName, parameter: 'lastName' }),
      new Like({ roleType: m.Person.UserEmail, parameter: 'email' }),
    ])
  );

  m.Organisation.filterDefinition = new FilterDefinition(new And([new Like({ roleType: m.Organisation.Name, parameter: 'name' })]));
}
