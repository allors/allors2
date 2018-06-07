import { AfterViewInit, Component, Input, Optional, QueryList, ViewChildren } from '@angular/core';
import { NgForm, NgModel } from '@angular/forms';

import { Field } from '../../../../angular';

@Component({
  selector: 'a-mat-datepicker',
  styleUrls: ["./datepicker.component.scss"],
  templateUrl: './datepicker.component.html',
})
export class AllorsMaterialDatepickerComponent extends Field {

  @Input()
  public useTime: boolean;

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }

  get hours(): number {
    if (this.model) {
      return this.model.getHours();
    }
  }

  set hours(value: number) {
    if (this.model) {
      this.model.setHours(value);
    }
  }

  get minutes(): number {
    if (this.model) {
      return this.model.getMinutes();
    }
  }

  set minutes(value: number) {
    if (this.model) {
      this.model.setMinutes(value);
    }
  }
}
