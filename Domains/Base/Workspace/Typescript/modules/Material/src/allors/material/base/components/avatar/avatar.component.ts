import { AfterViewInit, ChangeDetectorRef, Component, Input } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { AllorsMaterialDialogService } from '../../../base/services/dialog';
import { ISessionObject } from '../../../../framework';
import { MediaService } from '../../../../angular/base/content/MediaService';

@Component({
  selector: 'a-mat-avatar',
  templateUrl: './avatar.component.html',
})
export class AllorsMaterialAvatarComponent {
  constructor(public mediaService: MediaService) {
  }

  @Input() object: ISessionObject;
}
