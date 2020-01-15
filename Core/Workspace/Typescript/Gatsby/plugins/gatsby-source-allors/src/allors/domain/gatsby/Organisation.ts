import { domain } from '../domain';
import { Media } from '../generated/Media.g';
import { Meta } from '../../meta';
import { Organisation } from '../generated';

import createSlug from './createSlug';

declare module '../generated/Organisation.g' {
  interface Organisation {
    slug;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const organisation = workspace.constructorByObjectType.get(m.Organisation).prototype as any;

  Object.defineProperty(organisation, 'slug', {
    enumerable: true,
    get(this: Organisation): string {
      return createSlug(this.Name);
    },
  });
});
