import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from '../../../angular';

@Component({
  selector: 'a-md-slide-toggle',
  template: `
<div fxLayout="row">
  <md-slide-toggle fxFlex [(ngModel)]="model" [name]="name" [disabled]="!canWrite" [required]="required">
  {{label}}
  </md-slide-toggle>
</div>
`,
})
export class SlideToggleComponent extends Field {
}
