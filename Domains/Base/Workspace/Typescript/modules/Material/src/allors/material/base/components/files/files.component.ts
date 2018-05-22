import { Component, EventEmitter, Input, Optional, Output } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Field, MediaService } from '../../../../angular';
import { Media } from '../../../../domain';
import { ISession, ISessionObject } from '../../../../framework';

@Component({
  selector: 'a-mat-files',
  templateUrl: './files.component.html',
})
export class FilesComponent extends Field {
  @Output()
  public onChange: EventEmitter<Field> = new EventEmitter<Field>();

  @Input() public accept = 'image/*';

  public files: File[];

  constructor(@Optional() parentForm: NgForm, private mediaService: MediaService) {
    super(parentForm);
  }

  get medias(): Media[] {
    return this.model;
  }

  get fieldValue(): string {
    if (this.medias && this.medias.length > 0) {
      return this.medias.length + ' image' + (this.medias.length > 1 ? 's' : '');
    }

    return '';
  }

  public src(media: Media): string {
    if (media.InDataUri) {
      return media.InDataUri;
    } else if (media.UniqueId) {
      return this.mediaService.url(media);
    }
  }

  public deleteAll(): void {
    this.model = undefined;
    this.files = undefined;
  }

  public delete(media: Media): void {
    this.object.remove(this.roleType.name, media);
  }

  public onFileInput(event) {

    const fileList = event.srcElement.files;

    if (fileList.length && fileList.item) {
      for (let i = 0; i < fileList.length; i++) {
        this.addFile(fileList.item(i));
      }
    } else {
      this.addFile(fileList);
    }
  }

  // TODO: move to RxJS implementation
  private addFile(file: File) {
    const reader: FileReader = new FileReader();
    const load: () => void = () => {
      const media: Media = this.object.session.create('Media') as Media;
      media.FileName = file.name;
      media.InDataUri = reader.result;
      this.object.add(this.roleType.name, media);
    };

    reader.addEventListener('load', load, false);
    reader.readAsDataURL(file);

    this.onChange.emit(this);
  }
}
