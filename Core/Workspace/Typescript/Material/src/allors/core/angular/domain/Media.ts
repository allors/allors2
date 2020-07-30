import { domain } from '@allors/domain';
import { Media } from '@allors/domain/generated/Media.g';
import { Meta } from '@allors/meta';
import { assert } from '@allors/framework';

export const Download = 'download';
export const IsImage = 'isImage';

declare module @domain/generated/Media.g' {
  interface Media {
    [Download]: string;
    [IsImage]: boolean;
  }
}

domain.extend((workspace) => {
  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.Media);
  assert(cls);

  Object.defineProperty(cls.prototype, IsImage, {
    get(this: Media): boolean {
      const type = this.Type || this.InType;
      return type?.indexOf('image') === 0;
    },
  });
});
