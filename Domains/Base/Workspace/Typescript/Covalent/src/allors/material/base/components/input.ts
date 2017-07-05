import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from '../../../angular';

@Component({
  selector: 'a-md-input',
  template: `
<md-input-container fxLayout="column" fxLayoutAlign="top stretch">
  <input fxFlex mdInput [type]="textType" [(ngModel)]="model" [name]="name" [placeholder]="label" [required]="required" [disabled]="disabled" [readonly]="readonly">
  <md-hint *ngIf="hint">{{hint}}</md-hint>
</md-input-container>
`,
})
export class InputComponent extends Field {
}
