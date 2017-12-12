import { AfterViewInit, Component, Input, QueryList, ViewChildren } from "@angular/core";
import { NgForm, NgModel } from "@angular/forms";

import { Field } from "../../../../angular";

@Component({
  selector: "a-mat-datepicker",
  templateUrl: "./datepicker.component.html",
})
export class DatepickerComponent extends Field implements AfterViewInit {

  @Input()
  public useTime: boolean;

  @ViewChildren(NgModel)
  public controls: QueryList<NgModel>;

  constructor(private parentForm: NgForm) {
    super();
  }

  public ngAfterViewInit(): void {
    this.controls.forEach((control: NgModel) => {
      this.parentForm.addControl(control);
    });
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
