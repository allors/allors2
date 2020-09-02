import { Component, EventEmitter, Input, Optional, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

import { RoleField} from '@allors/angular/core';
import { ISession } from '@allors/domain/system';
import { Media } from '@allors/domain/generated';
import { MediaService } from '@allors/angular/services/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-file',
  templateUrl: './file.component.html',
})
export class AllorsMaterialFileComponent extends RoleField {
  @Output()
  changed: EventEmitter<RoleField> = new EventEmitter<RoleField>();

  @Input()
  accept = '*/*';

  constructor(@Optional() parentForm: NgForm, private dialog: MatDialog, private mediaService: MediaService) {
    super(parentForm);
  }

  get media(): Media {
    return this.model;
  }

  set media(value: Media) {
    this.model = value;
  }

  get fieldValue(): string {
    return this.media ? '1 file' : '';
  }

  get src(): string | null {
    if (this.media.InDataUri) {
      return this.media.InDataUri;
    } else if (this.media.UniqueId) {
      return this.mediaService.url(this.media);
    }

    return null;
  }

  public delete(): void {
    this.model = undefined;
  }

  public onFileInput(event: Event) {
    const input = event.target as HTMLInputElement;
    const files = input.files;
    if (files?.length && files.length > 0) {
      const file = files.item(0);

      if (file != null) {
        const session: ISession = this.object.session;
        const media = session.create('Media') as Media;
        media.InType = file.type;

        const reader: FileReader = new FileReader();
        const load: () => void = () => {
          media.InFileName = file.name;
          media.InDataUri = reader.result as string;
          this.media = media;
          this.changed.emit(this);
        };

        reader.addEventListener('load', load, false);
        reader.readAsDataURL(file);
      }
    }
  }
}
