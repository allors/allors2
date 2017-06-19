import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from '../../../angular';

@Component({
  selector: 'a-md-textarea',
  template: `
  <md-input-container fxLayout="row">
    <textarea fxFlex mdInput [(ngModel)]="model" [name]="name" [placeholder]="label" [disabled]="!canWrite" [required]="required">
    </textarea>
  </md-input-container>
`,
})
export class TextareaComponent extends Field {
}
