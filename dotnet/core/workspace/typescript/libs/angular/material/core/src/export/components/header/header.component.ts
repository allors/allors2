import { Component, Input } from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-header',
  templateUrl: './header.component.html',
})
export class AllorsMaterialHeaderComponent {
  @Input() title: string;
}
