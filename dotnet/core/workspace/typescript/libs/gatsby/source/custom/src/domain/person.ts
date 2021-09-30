import { Meta } from '@allors/meta/generated';
import { Person } from '@allors/domain/generated';
import { roles, createSlug } from '@allors/gatsby/source/core';

declare module '@allors/domain/generated' {
  interface Person {
    [roles.Slug]: string;
  }
}

export function extendPerson(workspace) {
  const m = workspace.metaPopulation as Meta;
  const organisation = workspace.constructorByObjectType.get(m.Person).prototype as any;

  Object.defineProperty(organisation, roles.Slug, {
    enumerable: true,
    get(this: Person): string {
      return createSlug(this.FullName);
    },
  });
}
