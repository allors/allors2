import { Injectable } from '@angular/core';
import { Media } from '../../../domain';
import { MediaConfig } from './media.config';

@Injectable()
export class MediaService {

  constructor(private config: MediaConfig) {
  }

  public url(media: Media): string {
    return `${this.config.url}media/${media.UniqueId}/${media.FileName}?revision=${media.Revision}`;
  }
}
