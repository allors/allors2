import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from '../../../angular';

@Component({
  selector: 'a-md-input',
  template: `
<md-input-container fxLayout="column" fxLayoutAlign="top stretch">
  <input fxFlex mdInput [type]="textType" [(ngModel)]="model" [name]="name" [placeholder]="label" [disabled]="!canWrite" [required]="required">
  <md-hint fxFlex *ngIf="hintText" style="white-space: nowrap">
     {{hintText}}
  </md-hint>
</md-input-container>
`,
})
export class InputComponent extends Field {

  @Input('hint')
  hintText: string;
}
