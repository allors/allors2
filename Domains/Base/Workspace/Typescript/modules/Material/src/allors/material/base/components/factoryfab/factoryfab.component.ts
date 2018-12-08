import { Component, OnInit, Input } from '@angular/core';

import { ObjectType, ObjectTypeRef } from '../../../../framework';
import { FactoryService } from '../../../../angular/base/factory';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-factory-fab',
  templateUrl: './factoryfab.component.html',
  styleUrls: ['./factoryfab.component.scss']
})
export class FactoryFabComponent implements OnInit {

  @Input() private objectType: ObjectType | ObjectTypeRef;

  classes: ObjectType[];

  constructor(public readonly factoryService: FactoryService) {
  }

  ngOnInit(): void {

    const objectType = this.objectType instanceof ObjectType ? this.objectType : this.objectType.objectType;

    if (objectType.isInterface) {
      this.classes = objectType.classes;
    } else {
      this.classes = [objectType];
    }
  }
}
