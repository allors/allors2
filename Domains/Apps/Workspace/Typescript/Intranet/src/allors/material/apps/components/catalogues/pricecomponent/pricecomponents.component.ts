import { Component, Input, Output, EventEmitter } from '@angular/core';

import { NavigationService, Allors } from '../../../../../angular';
import { PriceComponent } from '../../../../../domain';
import { ObjectType } from '../../../../../framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'price-component',
  templateUrl: './pricecomponents.component.html',
})
export class PriceComponentsComponent {

  @Input() pricecomponents: PriceComponent[];

  @Output() add: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() edit: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() delete: EventEmitter<PriceComponent> = new EventEmitter<PriceComponent>();

  constructor(public allors: Allors) { }
}
