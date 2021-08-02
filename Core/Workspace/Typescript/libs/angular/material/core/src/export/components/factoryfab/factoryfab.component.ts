import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { ObjectType } from '@allors/meta/system';
import { IObject } from '@allors/domain/system';

import { DatabaseService, WorkspaceService, Context } from '@allors/angular/services/core';
import { ObjectData, ObjectService } from '@allors/angular/material/services/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-factory-fab',
  templateUrl: './factoryfab.component.html',
  styleUrls: ['./factoryfab.component.scss']
})
export class FactoryFabComponent implements OnInit {

  @Input() objectType: ObjectType;

  @Input() createData: ObjectData;

  @Output() created: EventEmitter<IObject> = new EventEmitter();

  classes: ObjectType[];

  constructor(
    public readonly factoryService: ObjectService,
    private databaseService: DatabaseService,
    private workspaceService: WorkspaceService,
) {
  }

  ngOnInit(): void {

    if (this.objectType.isInterface) {
      this.classes = this.objectType.classes;
    } else {
      this.classes = [this.objectType];
    }

    const context = new Context(this.databaseService.database, this.workspaceService.workspace);
    this.classes = this.classes.filter((v) => this.factoryService.hasCreateControl(v, this.createData, context));
  }

  get dataAllorsActions(): string {
    return (this.classes) ? this.classes.map(v => v.name).join() : '';
  }

  create(objectType: ObjectType) {
    this.factoryService.create(objectType, this.createData)
      .subscribe((v) => {
        if (v && this.created) {
          this.created.next(v);
        }
      });
  }
}
