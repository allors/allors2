import { Component, EventEmitter, Input, Optional, Output } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ISessionObject } from '../../../../../framework';

import { RoleField, AssociationField } from '../../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-association-select',
  templateUrl: './select.component.html',
})
export class AllorsMaterialAssociationSelectComponent extends AssociationField {
  @Input()
  public display = 'display';

   @Output()
  public selected: EventEmitter<ISessionObject> = new EventEmitter();

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }

  public onModelChange(option: ISessionObject): void {
    this.selected.emit(option);
  }
}
