import { domain } from '../domain';
import { Media } from '../generated/Media.g';
import { Meta } from '../../meta';

declare module '../generated/Media.g' {
  interface Media {
    isImage;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const media = workspace.constructorByObjectType.get(m.Media).prototype as any;

  Object.defineProperty(media, 'isImage', {
    get(this: Media): boolean {
      const type = this.Type || this.InType;
      return type && type.indexOf('image') === 0;
    },
  });
});
