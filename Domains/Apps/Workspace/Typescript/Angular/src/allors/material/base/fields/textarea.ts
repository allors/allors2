import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from './Field';

@Component({
  selector: 'a-md-textarea',
  template: `
  <md-input-container flex>
    <textarea mdInput [(ngModel)]="model" [name]="name" [placeholder]="roleType.name" [disabled]="!canWrite" [required]="required">
    </textarea>
  </md-input-container>
`,
})
export class TextareaComponent extends Field {
}
