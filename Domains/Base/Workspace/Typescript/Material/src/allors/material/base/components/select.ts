import { Component, Input, AfterViewInit } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from '../../../angular';

@Component({
  selector: 'a-md-select',
  template: `
<div fxLayout="row">
  <md-select fxFlex [(ngModel)]="model" [name]="name" [placeholder]="roleType.name" [multiple]="roleType.isMany" [required]="required" [disabled]="disabled">
    <md-option *ngFor="let option of options" [value]="option">
      {{option[display]}}
    </md-option>
  </md-select>
  <md-hint *ngIf="hint">{{hint}}</md-hint>
</div>
`,
})
export class SelectComponent extends Field {
  @Input()
  display: string = 'display';

  @Input()
  options: ISessionObject[];
}
