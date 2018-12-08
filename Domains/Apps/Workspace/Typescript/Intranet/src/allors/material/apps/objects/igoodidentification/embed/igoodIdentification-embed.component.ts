import { Component, Input, Output, EventEmitter } from '@angular/core';

import { ContextService } from '../../../../../angular';
import { IGoodIdentification } from '../../../../../domain';
import { ObjectType, ObjectTypeRef } from '../../../../../framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'igoodidentification-embed',
  templateUrl: './igoodIdentification-embed.component.html',
})
export class IGoodIdentificationsComponent {

  @Input() igoodidentifications: IGoodIdentification[];

  @Output() add: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() edit: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() delete: EventEmitter<IGoodIdentification> = new EventEmitter<IGoodIdentification>();

  constructor(public allors: ContextService) { }
}
