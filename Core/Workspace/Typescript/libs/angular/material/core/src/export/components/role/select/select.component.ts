import { Component, EventEmitter, Input, Optional, Output } from '@angular/core';
import { NgForm } from '@angular/forms';

import { RoleField } from '@allors/angular/core';
import { ISessionObject } from '@allors/domain/system';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-select',
  templateUrl: './select.component.html',
})
export class AllorsMaterialSelectComponent extends RoleField {
  @Input()
  public display = 'display';

  @Input()
  public options: ISessionObject[];

  @Output()
  public selected: EventEmitter<ISessionObject> = new EventEmitter();

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }

  public onModelChange(option: ISessionObject): void {
    this.selected.emit(option);
  }

  onRestore(event: Event) {
    event.stopPropagation();
    this.restore()
    this.selected.emit(this.model);
  }
}
