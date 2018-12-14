import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { ObjectType } from '../../../../framework';
import { CreateData, ObjectData } from '../../services/object';
import { ObjectService } from '../../services/object';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-factory-fab',
  templateUrl: './factoryfab.component.html',
  styleUrls: ['./factoryfab.component.scss']
})
export class FactoryFabComponent implements OnInit {

  @Input() private objectType: ObjectType;

  @Input() private createData: CreateData;

  @Output() private created: EventEmitter<ObjectData> = new EventEmitter();;

  classes: ObjectType[];

  constructor(public readonly factoryService: ObjectService) {
  }

  ngOnInit(): void {

    if (this.objectType.isInterface) {
      this.classes = this.objectType.classes;
    } else {
      this.classes = [this.objectType];
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
