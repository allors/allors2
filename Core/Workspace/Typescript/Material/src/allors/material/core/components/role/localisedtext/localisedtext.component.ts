import { Component, Input, OnChanges, Optional , SimpleChange, SimpleChanges } from '@angular/core';
import { NgForm } from '@angular/forms';

import { humanize, LocalisedRoleField } from '../../../../../angular';
import { Locale, LocalisedText } from '../../../../../domain';
import { assert } from 'src/allors/framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-localised-text',
  templateUrl: './localisedtext.component.html',
})
export class AllorsMaterialLocalisedTextComponent extends LocalisedRoleField {

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
