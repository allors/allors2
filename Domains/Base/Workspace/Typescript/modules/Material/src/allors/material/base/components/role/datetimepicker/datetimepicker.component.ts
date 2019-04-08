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
      const model = moment.utc(this.model);
      return model.hours();
    }
  }

  set hours(value: number) {
    if (this.model) {
      const model = moment.utc(this.model);
      model.hours(value);
      this.model = model;
    }
  }

  get minutes(): number {
    if (this.model) {
      const model = moment.utc(this.model);
      return model.minutes();
    }
  }

  set minutes(value: number) {
    if (this.model) {
      const model = moment.utc(this.model);
      model.minutes(value);
      this.model = model;
    }
  }

  public onModelChange(selected: Date): void {
    this.selected.emit(selected);
  }
}
