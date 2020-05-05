import * as moment from 'moment/moment';
import { Component, Optional, Output, EventEmitter, Self, Inject } from '@angular/core';
import { NgForm } from '@angular/forms';
import { DateAdapter, MAT_DATE_LOCALE } from '@angular/material/core';
import { MomentDateAdapter } from '@angular/material-moment-adapter';

import { RoleField } from '../../../../../angular';

export function dateAdapterFactory(dateLocale: string) {
  return new MomentDateAdapter(dateLocale, { useUtc: true });
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-datepicker',
  styleUrls: ['./datepicker.component.scss'],
  templateUrl: './datepicker.component.html',
  providers: [
    {
      provide: DateAdapter, useFactory: dateAdapterFactory, deps: [MAT_DATE_LOCALE]
    },
  ]
})
export class AllorsMaterialDatepickerComponent extends RoleField {

  @Output()
  public selected: EventEmitter<Date> = new EventEmitter();

  private momentModel: moment.Moment | null = null;

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
