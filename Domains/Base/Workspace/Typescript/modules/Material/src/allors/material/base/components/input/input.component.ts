import { Component, Optional } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Field } from '../../../../angular';

@Component({
  selector: 'a-mat-input',
  templateUrl: './input.component.html',
})
export class AllorsMaterialInputComponent extends Field {

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
