import { Component, Optional, Output, EventEmitter, Self } from '@angular/core';
import { NgForm } from '@angular/forms';

import { RoleField } from '@allors/angular/core';

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

  get hours(): number | null {
    return this.model ? new Date(this.model).getHours() : null;
  }

  set hours(value: number | null) {
    if (this.model) {
      var date = new Date(this.model);
      date.setHours(value);
      this.model = date.toISOString();
    }
  }

  get minutes(): number | null {
    return this.model ? new Date(this.model).getMinutes() : null;
  }

  set minutes(value: number | null) {
    if (this.model) {
      var date = new Date(this.model);
      date.setMinutes(value);
      this.model = date.toISOString();
    }
  }

  public onModelChange(selected: Date): void {
    this.selected.emit(selected);
  }
}
