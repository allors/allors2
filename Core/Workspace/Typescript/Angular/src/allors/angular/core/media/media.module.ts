import { NgModule, ModuleWithProviders } from '@angular/core';
import { MediaConfig } from './media.config';

import { MediaService } from './media.service';
export { MediaService } from './media.service';

@NgModule({
  declarations: [
  ],
  exports: [
  ],
  imports: [
  ],
  providers: [
    MediaService
  ]
})
export class MediaModule {
  static forRoot(config: MediaConfig): ModuleWithProviders {
    return {
      ngModule: MediaModule,
      providers: [
        MediaService,
        { provide: MediaConfig, useValue: config },
      ]
    };
  }
}
