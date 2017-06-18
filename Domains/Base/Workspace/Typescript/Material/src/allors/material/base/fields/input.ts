import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from '../../../angular';

@Component({
  selector: 'a-md-input',
  template: `
<md-input-container>
  <input mdInput [type]="textType" [(ngModel)]="model" [name]="name" [placeholder]="label" [disabled]="!canWrite" [required]="required">
</md-input-container>
`,
})
export class InputComponent extends Field {
}
