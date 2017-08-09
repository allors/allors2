import { Component, Input , ChangeDetectorRef } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from '../../../angular';

@Component({
  selector: 'a-md-checkbox',
  template: `
<div fxLayout="row">
  <md-checkbox fxFlex [(ngModel)]="model" [name]="name" [required]="required" [disabled]="disabled">
  {{label}}
  </md-checkbox>
  <md-hint *ngIf="hint">{{hint}}</md-hint>
</div>
`,
})
export class CheckboxComponent extends Field {
}
