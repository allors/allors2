import * as moment from 'moment';

import { Component, Optional, Output, EventEmitter, Self, Inject } from '@angular/core';
import { NgForm } from '@angular/forms';
import { DateAdapter, MAT_DATE_LOCALE } from '@angular/material/core';
import { MomentDateAdapter } from '@angular/material-moment-adapter';

import { RoleField, ModelField } from '../../../../../angular';

export function dateAdapterFactory(dateLocale: string) {
  return new MomentDateAdapter(dateLocale, { useUtc: false });
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-datetimepicker',
  styleUrls: ['./datetimepicker.component.scss'],
  templateUrl: './datetimepicker.component.html',
  providers: [
    {
      provide: DateAdapter, useFactory: dateAdapterFactory, deps: [MAT_DATE_LOCALE]
    },
  ]
})
export class AllorsMaterialDatetimepickerComponent extends RoleField {

  @Output()
  public selected: EventEmitter<Date> = new EventEmitter();

  private momentModel: moment.Moment | null = null;

  private adapter: MomentDateAdapter;

  constructor(
    @Optional() parentForm: NgForm,
    @Self() dateAdapter: DateAdapter<any>,
  ) {
    super(parentForm);

    this.adapter = dateAdapter as MomentDateAdapter;
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

  get hours(): number | null {
    return this.model?.hours() ?? null;
  }

  set hours(value: number | null) {
    if (this.model && value !== null) {
      this.model = this.model.clone().hours(value);
    }
  }

  get minutes(): number | null {
    if (this.model) {
      return this.model?.minutes();
    }

    return null;
  }

  set minutes(value: number | null) {
    if (this.model && value !== null) {
      this.model = this.model.clone().minutes(value);
    }
  }

  public onModelChange(selected: Date): void {
    this.selected.emit(selected);
  }
}
