import { domain } from '../domain';
import { Media } from '../generated/Media.g';
import { Meta } from '../../meta';
import { assert } from '../../framework';

declare module '../generated/Media.g' {
  interface Media {
    isImage: boolean;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const mediaClass = workspace.constructorByObjectType.get(m.Media);
  assert(mediaClass);

  Object.defineProperty(mediaClass.prototype, 'isImage', {
    get(this: Media): boolean {
      const type = this.Type || this.InType;
      return type?.indexOf('image') === 0;
    },
  });
});
