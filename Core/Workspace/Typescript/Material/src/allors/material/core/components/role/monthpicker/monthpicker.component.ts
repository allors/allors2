import * as moment from 'moment';
import { Moment } from 'moment';

import { Component, Optional, Output, EventEmitter, Self, Inject, Input, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { DateAdapter } from '@angular/material/core';
import { MomentDateAdapter } from '@angular/material-moment-adapter';

import { RoleField } from '../../../../../angular';
import { MatDatepicker } from '@angular/material/datepicker';

export class MonthDateAdapter extends MomentDateAdapter {
  format(date: Moment, displayFormat: string): string {
    return date.format('MMMM YYYY');
  }
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-monthpicker',
  styleUrls: ['./monthpicker.component.scss'],
  templateUrl: './monthpicker.component.html',
  providers: [
    {
      provide: DateAdapter, useClass: MonthDateAdapter
    }
  ]
})
export class AllorsMaterialMonthpickerComponent extends RoleField {

  @Output()
  public selected: EventEmitter<Date> = new EventEmitter();

  @ViewChild(MatDatepicker) picker;

  private momentModel: moment.Moment | null = null;

  constructor(
    @Optional() parentForm: NgForm,
    @Self() dateAdapter: DateAdapter<any>,
  ) {
    super(parentForm);
  }
  public monthSelected(params) {
    this.model = params;
    this.picker.close();
  }
  public onModelChange(selected: Date): void {
    this.selected.emit(selected);
  }

  get model(): any {
    if (this.momentModel == null) {
      if (this.ExistObject) {
        const isoString = this.object.get(this.roleType);
        this.momentModel = isoString ? moment(isoString).clone() : null;
      }
    }

    return this.momentModel;
  }

  set model(value: any) {
    if (!this.ExistObject) {
      this.momentModel = null;
    } else {

      this.momentModel = value;

      if (value === null) {
        this.object.set(this.roleType, null);
      } else {
        if (value.isValid()) {
          const isoString = value.toISOString();
          this.object.set(this.roleType, isoString);
        }
      }
    }
  }
}
