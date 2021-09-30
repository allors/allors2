import { Meta } from "@allors/meta/generated";
import { Media } from '@allors/domain/generated';

export const IsPdf = 'isPdf';

declare module '@allors/domain/generated' {
  interface Media {
    [IsPdf]: boolean;
  }
}

export function extendMedia(workspace) {
  const m = workspace.metaPopulation as Meta;
  const objectType = workspace.constructorByObjectType.get(m.Media).prototype as any;

  Object.defineProperty(objectType, IsPdf, {
    enumerable: true,
    get(this: Media): boolean {
      const type = this.Type.toLowerCase();
      return type === 'application/pdf';
    },
  });
}
