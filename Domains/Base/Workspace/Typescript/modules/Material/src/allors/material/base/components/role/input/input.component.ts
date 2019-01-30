import { Component, Optional } from '@angular/core';
import { NgForm } from '@angular/forms';

import { RoleField } from '../../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-input',
  templateUrl: './input.component.html',
})
export class AllorsMaterialInputComponent extends RoleField {

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
