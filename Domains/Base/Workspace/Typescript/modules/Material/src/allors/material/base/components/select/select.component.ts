import { Component, EventEmitter, Input, Optional, Output } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ISessionObject } from '../../../../framework';

import { Field } from '../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-select',
  templateUrl: './select.component.html',
})
export class AllorsMaterialSelectComponent extends Field {
  @Input()
  public display = 'display';

  @Input()
  public options: ISessionObject[];

  @Output()
  public onSelect: EventEmitter<ISessionObject> = new EventEmitter();

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }

  public onModelChange(option: ISessionObject): void {
    this.onSelect.emit(option);
  }
}
