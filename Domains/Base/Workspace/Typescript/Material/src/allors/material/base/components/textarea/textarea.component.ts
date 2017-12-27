import { AfterViewInit, Component, Optional, QueryList, ViewChildren } from "@angular/core";
import { NgForm, NgModel } from "@angular/forms";

import { Field } from "../../../../angular";

@Component({
  selector: "a-mat-textarea",
  templateUrl: "./textarea.component.html",
})
export class TextareaComponent extends Field {

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
