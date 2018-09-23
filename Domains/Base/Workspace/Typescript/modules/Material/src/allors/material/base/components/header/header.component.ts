import { Component, Self, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-header',
  templateUrl: './header.component.html',
})
export class AllorsMaterialHeaderComponent {
  @Input() title: string;
}
