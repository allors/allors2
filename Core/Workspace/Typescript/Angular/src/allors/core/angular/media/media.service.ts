import { Injectable } from '@angular/core';
import { Media } from '@allors/domain';
import { MediaConfig } from './media.config';

@Injectable()
export class MediaService {

  constructor(private config: MediaConfig) {
  }

  public url(media: Media): string {
    if (media.FileName) {
      const fileName = encodeURI(media.FileName);
      return `${this.config.url}media/${media.UniqueId}/${media.Revision}/${fileName}`;
    } else {
      return `${this.config.url}media/${media.UniqueId}/${media.Revision}`;
    }
  }
}
