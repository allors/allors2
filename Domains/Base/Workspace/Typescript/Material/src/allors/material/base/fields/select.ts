import { Component, Input, AfterViewInit } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from './Field';

@Component({
  selector: 'a-md-select',
  template: `
<div flex>
  <md-select [(ngModel)]="model" [name]="name" [placeholder]="roleType.name" [disabled]="!canWrite" [required]="required" [multiple]="roleType.isMany">
    <md-option *ngFor="let option of options" [value]="option">
      {{option[display]}}
    </md-option>
  </md-select>
</div>
`,
})
export class SelectComponent extends Field {
  @Input()
  display: string = 'display';

  @Input()
  options: ISessionObject[];

}
