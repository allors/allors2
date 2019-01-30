import { Component, Optional } from '@angular/core';
import { NgForm } from '@angular/forms';

import { RoleField } from '../../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-textarea',
  templateUrl: './textarea.component.html',
})
export class TextareaComponent extends RoleField {

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
