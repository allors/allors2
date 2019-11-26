import { Component, EventEmitter, Input, Optional, Output } from '@angular/core';
import { NgForm } from '@angular/forms';

import { RoleField, MediaService } from '../../../../../angular';
import { Media } from '../../../../../domain';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-files',
  templateUrl: './files.component.html',
})
export class AllorsMaterialFilesComponent extends RoleField {
  @Output()
  changed: EventEmitter<RoleField> = new EventEmitter<RoleField>();

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
      return this.medias.length + ' file' + (this.medias.length > 1 ? 's' : '');
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
    this.object.remove(this.roleType, media);
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
      media.InDataUri = reader.result as string;
      this.object.add(this.roleType, media);
    };

    reader.addEventListener('load', load, false);
    reader.readAsDataURL(file);

    this.changed.emit(this);
  }
}
