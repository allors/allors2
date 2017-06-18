import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from './Field';

@Component({
  selector: 'a-md-datepicker',
  template: `
<md-input-container flex>
  <input mdInput [mdDatepicker]="picker"
      [(ngModel)]="model" [name]="name" [placeholder]="label" [disabled]="!canWrite" [required]="required">
  <button mdSuffix [mdDatepickerToggle]="picker"></button>
</md-input-container>
<md-datepicker #picker></md-datepicker>
`,
})
export class DatepickerComponent extends Field {
}
