import { Component, Optional } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Field } from '../../../../angular';

@Component({
  selector: 'a-mat-slide-toggle',
  templateUrl: './slidetoggle.component.html',
})
export class AllorsMaterialSlideToggleComponent extends Field {

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
