import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { ObjectType, ObjectTypeRef } from '../../../../framework';
import { ObjectService } from '../../../../angular/base/object';
import { ObjectData } from 'src/allors/angular/base/object/object.data';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-factory-fab',
  templateUrl: './factoryfab.component.html',
  styleUrls: ['./factoryfab.component.scss']
})
export class FactoryFabComponent implements OnInit {

  @Input() private objectType: ObjectType | ObjectTypeRef;

  @Input() private createData: any;

  @Output() private created: EventEmitter<ObjectData> = new EventEmitter();;

  classes: ObjectType[];

  constructor(public readonly factoryService: ObjectService) {
  }

  ngOnInit(): void {

    const objectType = this.objectType instanceof ObjectType ? this.objectType : this.objectType.objectType;

    if (objectType.isInterface) {
      this.classes = objectType.classes;
    } else {
      this.classes = [objectType];
    }

    this.classes = this.classes.filter((v) => this.factoryService.hasCreateControl(v));
  }

  create(objectType: ObjectType) {
    this.factoryService.create(objectType, this.createData)
      .subscribe((v) => {
        this.created.next(v);
      });
  }
}
