import { AfterViewInit, Component, OnDestroy, Optional, QueryList, ViewChildren } from "@angular/core";
import { NgForm, NgModel } from "@angular/forms";

import { Field } from "../../../../angular";

@Component({
  selector: "a-mat-input",
  templateUrl: "./input.component.html",
})
export class InputComponent extends Field {

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
