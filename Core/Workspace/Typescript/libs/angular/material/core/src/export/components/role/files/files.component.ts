import { Component, EventEmitter, Input, Optional, Output } from '@angular/core';
import { NgForm } from '@angular/forms';

import { RoleField } from '@allors/angular/core';
import { Media } from '@allors/domain/generated';
import { MediaService } from '@allors/angular/services/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-files',
  templateUrl: './files.component.html',
})
export class AllorsMaterialFilesComponent extends RoleField {
  @Output()
  changed: EventEmitter<RoleField> = new EventEmitter<RoleField>();

  @Input() public accept = 'image/*';

  public files: File[] | null;

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

  public src(media: Media): string | null {
    if (media.InDataUri) {
      return media.InDataUri;
    } else if (media.UniqueId) {
      return this.mediaService.url(media);
    }

    return null;
  }

  public deleteAll(): void {
    this.model = null;
    this.files = null;
  }

  public delete(media: Media): void {
    this.object.remove(this.roleType, media);
  }

  public onFileInput(event: Event) {

    const input = (event.target as HTMLInputElement);
    const files = input.files;
    if (files?.length && files.length > 0) {
      for (let i = 0; i < files.length; i++) {
        const file = files.item(i);
        if (file != null) {
          this.addFile(file);
        }
      }
    }
  }

  // TODO: move to RxJS implementation and share with file.component
  private addFile(file: File) {
    const reader: FileReader = new FileReader();
    const load: () => void = () => {
      const media: Media = this.object.session.create('Media') as Media;
      media.InFileName = file.name;
      media.InDataUri = reader.result as string;
      this.object.add(this.roleType, media);
    };

    reader.addEventListener('load', load, false);
    reader.readAsDataURL(file);

    this.changed.emit(this);
  }
}
