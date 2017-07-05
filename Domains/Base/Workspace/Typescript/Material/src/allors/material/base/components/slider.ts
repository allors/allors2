import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from '../../../angular';

@Component({
  selector: 'a-md-input',
  template: `
<md-input-container fxLayout="column" fxLayoutAlign="top stretch">
  <md-slider fxFlex [(ngModel)]="model" [name]="name" [invert]="invert" [min]="min" [max]="max" [step]="step" [thumbLabel]="thumbLabel" [tickInterval]="tickInterval" [vertical]="vertical" [color]="color" [required]="required" [disabled]="disabled"></md-slider>

  <input fxFlex mdInput [type]="textType"    >
  <md-hint *ngIf="hint">{{hint}}</md-hint>
</md-input-container>
`,
})
export class SliderComponent extends Field {

  @Input()
  invert: boolean;

  @Input()
  max: number;

  @Input()
  min: number;

  @Input()
  step: number;

  @Input()
  thumbLabel: boolean;

  @Input()
  tickInterval: 'auto' | number;

  @Input()
  vertical: boolean;

  @Input()
  color: 'primary' | 'accent' | 'warn' = 'accent';
}
