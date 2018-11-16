import { Component, Input, Output, EventEmitter } from '@angular/core';

import { NavigationService, Allors } from '../../../../../../angular';
import { MetaDomain } from '../../../../../../meta';
import { SerialisedItem, Part, Party } from '../../../../../../domain';
import { ObjectType } from '../../../../../../framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialised-items',
  templateUrl: './serialiseditems.component.html',
})
export class SerialisedItemsComponent {

  @Input() part: Part;

  @Input() owner: Party;

  @Input() serialisedItems: SerialisedItem[];

  @Output() add: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() edit: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() delete: EventEmitter<SerialisedItem> = new EventEmitter<SerialisedItem>();

  m: MetaDomain;

  constructor(
    public allors: Allors,
    public navigationService: NavigationService) {
      this.m = allors.m;
     }
}
