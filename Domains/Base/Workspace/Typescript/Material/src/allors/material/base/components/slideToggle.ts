import { Component, Input, ChangeDetectorRef } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from '../../../angular';

@Component({
  selector: 'a-md-slide-toggle',
  template: `
<div fxLayout="row">
  <md-slide-toggle fxFlex [(ngModel)]="model" [name]="name" [required]="required" [disabled]="disabled" [checked]="checked">
  {{label}}
  </md-slide-toggle>
  <md-hint *ngIf="hint">{{hint}}</md-hint>
</div>
`,
})
export class SlideToggleComponent extends Field {

  @Input()
  checked: boolean;
}
