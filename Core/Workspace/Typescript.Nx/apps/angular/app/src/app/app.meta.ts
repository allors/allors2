import { MetaPopulation } from '@allors/workspace/meta';
import { And, Like } from '@allors/workspace/data';

import { FilterDefinition } from '@allors/angular/core';

import { Meta } from '@allors/meta';

export function appMeta(metaPopulation: MetaPopulation) {
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
