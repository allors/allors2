import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from './Field';

@Component({
  selector: 'a-md-select',
  template: `
<md-input-container flex>
  <md-select [(ngModel)]="model" [name]="name" [placeholder]="roleType.name" [disabled]="!canWrite" [required]="required">
    <md-option *ngFor="let option of options" [value]="option">
      {{option[display]}}
    </md-option>
  </md-select>
</md-input-container>
`,
})
export class SelectComponent extends Field {
  @Input()
  display = 'display';

  @Input()
  options: ISessionObject[];
}
