import { Injectable } from '@angular/core';
import { Media } from '@allors/domain/generated';

@Injectable()
export abstract class MediaService {
  abstract url(media: Media): string;
}
