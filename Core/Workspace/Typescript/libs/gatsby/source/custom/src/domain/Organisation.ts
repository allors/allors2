import { Meta } from "@allors/meta/generated";
import { Organisation } from '@allors/domain/generated';
import { createSlug } from '@allors/gatsby/source/core';
import { roles } from '@allors/gatsby/source/core';

declare module '@allors/domain/generated' {
  interface Organisation {
    [roles.Slug]: string;
  }
}

export function extendOrganisation(workspace)  {

  const m = workspace.metaPopulation as Meta;
  const organisation = workspace.constructorByObjectType.get(m.Organisation).prototype as any;

  Object.defineProperty(organisation, roles.Slug, {
    enumerable: true,
    get(this: Organisation): string {
      return createSlug(this.Name);
    },
  });
};
