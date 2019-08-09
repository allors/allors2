import { Meta } from '../allors/meta';
import { MetaPopulation } from '../allors/framework';

export function appMeta(metaPopulation: MetaPopulation) {

  const m = metaPopulation as Meta;

  m.Person.list = '/contacts/people';
  m.Person.overview = '/contacts/person/:id';
  m.Organisation.list = '/contacts/organisations';
  m.Organisation.overview = '/contacts/organisation/:id';
}
