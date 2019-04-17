import * as moment from 'moment';

import { Component, Optional, Output, EventEmitter, Self, Inject } from '@angular/core';
import { NgForm } from '@angular/forms';
import { DateAdapter, MAT_DATE_LOCALE } from '@angular/material';
import { MomentDateAdapter } from '@angular/material-moment-adapter';

import { RoleField, ModelField } from '../../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-datetimepicker',
  styleUrls: ['./datetimepicker.component.scss'],
  templateUrl: './datetimepicker.component.html',
  providers: [
    {
      provide: DateAdapter, useFactory: (dateLocale) => {
        return new MomentDateAdapter(dateLocale, { useUtc: false })
      }, deps: [MAT_DATE_LOCALE]
    },
  ]
})
export class AllorsMaterialDatetimepickerComponent extends RoleField {

  @Output()
  public selected: EventEmitter<Date> = new EventEmitter();

  private momentModel: moment.Moment;

  constructor(
    @Optional() parentForm: NgForm,
    @Self() dateAdapter: DateAdapter<any>,
  ) {
    super(parentForm);
  }

  get model(): any {
    if (!this.momentModel) {
      this.momentModel = this.ExistObject ? moment(this.object.get(this.roleType.name)) : undefined;
    }

    return this.momentModel;
  }

  set model(value: any) {
    if (this.ExistObject) {
      if(!value){

        this.momentModel = null;
        this.object.set(this.roleType.name, null);
        console.log(this.momentModel.utc().toISOString());

      } else{

        this.momentModel = value;

        if (this.momentModel.isValid()) {
          this.object.set(this.roleType.name, this.momentModel.utc().toISOString());
          this.momentModel = null;

          console.log(this.momentModel.utc().toISOString());
        }
      }
    }
  }


  get hours(): number {
    if (this.model) {
      return this.model.hours();
    }
  }

  set hours(value: number) {
    if (this.model) {
      this.model.hours(value);
      this.model = this.model;
    }
  }

  get minutes(): number {
    if (this.model) {
      return this.model.minutes();
    }
  }

  set minutes(value: number) {
    if (this.model) {
      this.model.minutes(value);
      this.model = this.model;
    }
  }

  public onModelChange(selected: Date): void {
    this.selected.emit(selected);
  }
}
