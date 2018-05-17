import { Component, EventEmitter, Input, Optional, Output } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Field, MediaService } from '../../../../angular';
import { Media } from '../../../../domain';
import { ISession, ISessionObject } from '../../../../framework';

@Component({
  selector: 'a-mat-file',
  styleUrls: ['./file.component.scss'],
  templateUrl: './file.component.html',
})
export class FileComponent extends Field {
  @Output() public onChange: EventEmitter<Field> = new EventEmitter<Field>();

  @Input() public accept = 'image/*';

  public onFileInput(event) {

    event.stopPropagation();

    const files = event.srcElement.files;
    const file  = files[0];

    if (this.ExistObject) {
      if (!this.model) {
        const session: ISession = this.object.session;
        this.model = session.create('Media');
      }
    }

    const reader: FileReader = new FileReader();
    const load: () => void = () => {
      this.media.FileName = file.name;
      this.media.InDataUri = reader.result;
      this.onChange.emit(this);
    };

    reader.addEventListener('load', load, false);
    reader.readAsDataURL(file);
  }

  constructor(@Optional() parentForm: NgForm, private mediaService: MediaService) {
    super(parentForm);
  }

  get media(): Media {
    return this.model;
  }

  get fieldValue(): string {
    return this.media ? '1 image' : '';
  }

  get src(): string {
    if (this.media) {
      if (this.media.InDataUri) {
        return this.media.InDataUri;
      } else if (this.media.UniqueId) {
        return this.mediaService.url(this.media);
      }
    }
  }

  public delete(): void {
    this.model = undefined;
  }
}
