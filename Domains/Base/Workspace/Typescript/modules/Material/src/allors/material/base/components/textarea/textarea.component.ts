import { Component, Optional } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Field } from '../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-textarea',
  templateUrl: './textarea.component.html',
})
export class TextareaComponent extends Field {

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
