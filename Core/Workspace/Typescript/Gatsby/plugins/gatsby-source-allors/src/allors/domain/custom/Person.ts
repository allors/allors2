import { domain } from '../domain';
import { Meta } from '../../meta';
import { Person } from '../generated';

import { createSlug } from '../../gatsby/utils/createSlug';

declare module '../generated/Person.g' {
  interface Person {
    slug;
  }
}

export const Slug = 'slug';

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const organisation = workspace.constructorByObjectType.get(m.Person).prototype as any;

  Object.defineProperty(organisation, Slug, {
    enumerable: true,
    get(this: Person): string {
      return createSlug(this.FullName);
    },
  });
});
