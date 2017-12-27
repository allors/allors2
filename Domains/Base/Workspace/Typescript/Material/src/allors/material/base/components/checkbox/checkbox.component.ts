import { Component, Optional } from "@angular/core";

import { NgForm } from "@angular/forms";
import { Field } from "../../../../angular";

@Component({
  selector: "a-mat-checkbox",
  templateUrl: "./checkbox.component.html",
})
export class CheckboxComponent extends Field {

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
