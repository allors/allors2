import { Component, Input, Output, EventEmitter } from '@angular/core';

import { NavigationService, Allors } from '../../../../../angular';
import { IGoodIdentification } from '../../../../../domain';
import { ObjectType } from '../../../../../framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'igood-identifications',
  templateUrl: './igoodidentifications.component.html',
})
export class IGoodIdentificationsComponent {

  @Input() igoodidentifications: IGoodIdentification[];

  @Output() add: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() edit: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() delete: EventEmitter<IGoodIdentification> = new EventEmitter<IGoodIdentification>();

  constructor(public allors: Allors) { }
}
