import { Component, Optional, Output, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';

import { RoleField } from '../../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-datetimepicker',
  styleUrls: ['./datetimepicker.component.scss'],
  templateUrl: './datetimepicker.component.html',
})
export class AllorsMaterialDatetimepickerComponent extends RoleField {

  @Output()
  public selected: EventEmitter<Date> = new EventEmitter();

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }

  get hours(): number {
    if (this.model) {
      if (this.model.hour) {
        return this.model.hour();
      }

      return this.model.getHours();
    }
  }

  set hours(value: number) {
    if (this.model) {
      if (this.model.hour) {
        this.model = new Date(this.model.hour(value));
      } else {
        this.model = new Date(this.model.setHours(value));
      }
    }
  }

  get minutes(): number {
    if (this.model) {
      if (this.model.minute) {
        return this.model.minute();
      }
      return this.model.getMinutes();
    }
  }

  set minutes(value: number) {
    if (this.model) {
      if (this.model.minute) {
        this.model = new Date(this.model.minute(value));
      } else {
        this.model = new Date(this.model.setMinutes(value));
      }
    }
  }

  public onModelChange(selected: Date): void {
    this.selected.emit(selected);
  }
}
