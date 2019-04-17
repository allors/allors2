import * as moment from 'moment';

import { Component, Optional, Output, EventEmitter, Self, Inject } from '@angular/core';
import { NgForm } from '@angular/forms';
import { DateAdapter, MAT_DATE_LOCALE } from '@angular/material';
import { MomentDateAdapter } from '@angular/material-moment-adapter';

import { RoleField } from '../../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-datepicker',
  styleUrls: ['./datepicker.component.scss'],
  templateUrl: './datepicker.component.html',
  providers: [
    {
      provide: DateAdapter, useFactory: (dateLocale) => {
        return new MomentDateAdapter(dateLocale, { useUtc: true })
      }, deps: [MAT_DATE_LOCALE]
    },
  ]
})
export class AllorsMaterialDatepickerComponent extends RoleField {

  @Output()
  public selected: EventEmitter<Date> = new EventEmitter();

  private momentModel: moment.Moment;

  constructor(
    @Optional() parentForm: NgForm,
    @Self() dateAdapter: DateAdapter<any>,
  ) {
    super(parentForm);
  }

  public onModelChange(selected: Date): void {
    this.selected.emit(selected);
  }

  get model(): any {
    if (this.momentModel) {
      return this.momentModel;
    }

    return this.ExistObject ? this.object.get(this.roleType.name) : undefined;
  }

  set model(value: any) {
    if (this.ExistObject) {
      if(!value){

        this.momentModel = null;
        this.object.set(this.roleType.name, null);
        console.log(this.momentModel.utc().toISOString());

      } else{

        this.momentModel = value as moment.Moment;

        if (this.momentModel.isValid()) {
          this.object.set(this.roleType.name, this.momentModel.utc().toISOString());
          this.momentModel = null;

          console.log(this.momentModel.utc().toISOString());
        }
      }
    }
  }

}
