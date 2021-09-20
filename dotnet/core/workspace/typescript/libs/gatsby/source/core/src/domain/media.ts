import { Meta } from '@allors/meta/generated';
import { Media } from '@allors/domain/generated';
import { assert } from '@allors/meta/system';

export const Download = 'download';
export const IsImage = 'isImage';

declare module '@allors/domain/generated' {
  interface Media {
    [Download]: string;
    [IsImage]: boolean;
  }
}

export function extendMedia(workspace) {
  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.Media);
  assert(cls);

  Object.defineProperty(cls.prototype, IsImage, {
    get(this: Media): boolean {
      const type = this.Type || this.InType;
      return type?.indexOf('image') === 0;
    },
  });
}
