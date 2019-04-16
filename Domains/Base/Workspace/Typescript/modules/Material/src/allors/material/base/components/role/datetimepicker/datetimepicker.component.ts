import * as moment from 'moment';

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
      const wrapped = moment.utc(this.model).local();
      const x = wrapped.hours();
      return x;
    }
  }

  set hours(value: number) {
    if (this.model) {
      const wrapped = moment.utc(this.model).local();
      wrapped.hours(value);
      this.model = wrapped.utc();
    }
  }

  get minutes(): number {
    if (this.model) {
      const wrapped = moment.utc(this.model).local();
      return wrapped.minutes();
    }
  }

  set minutes(value: number) {
    if (this.model) {
      const wrapped = moment.utc(this.model).local();
      wrapped.minutes(value);
      this.model = wrapped.utc();
    }
  }

  public onModelChange(selected: Date): void {
    this.selected.emit(selected);
  }
}
