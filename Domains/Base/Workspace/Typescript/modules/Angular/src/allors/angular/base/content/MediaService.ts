import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Media } from '../../../domain';
import { DatabaseConfig } from '../framework/DatabaseConfig';

@Injectable()
export class MediaService {

  constructor(private databaseConfig: DatabaseConfig) {}

  public url(media: Media): string {
    return `${this.databaseConfig.url}Media/Download/${media.UniqueId}?revision=${media.Revision}`;
  }

  public display(media: Media): void {
    const newWindow = window.open();
    newWindow.location.href = this.url(media);
  }
}
