import { Component, Input } from '@angular/core';
import { TdMediaService } from '@covalent/core';

@Component({
  selector: 'relations-toolbar',
  template: `
 <button md-icon-button tdLayoutNavListOpen [hideWhenOpened]="true">
  <md-icon>arrow_back</md-icon>
</button>
<span>{{title}}</span>
`,
})
export class RelationsToolbarComponent {
  @Input()
  title: string;

  constructor(public media: TdMediaService) { }
}
