import { Component, Input, ViewChild, OnInit } from '@angular/core';

import { BaseTable } from './BaseTable';
import { Column } from './Column';
import { TableRow } from './TableRow';
import { MatSort } from '@angular/material/sort';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss']
})
export class AllorsMaterialTableComponent implements OnInit {

  @Input()
  public table: BaseTable;

  @ViewChild(MatSort, { static: true }) matSort: MatSort;

  ngOnInit(): void {
    this.table.init(this.matSort);
  }

  cellStyle(row: TableRow, column: Column) {
    return this.action(row, column) ? 'pointer' : null;
  }

  onCellClick(row: TableRow, column: Column) {

    const action = this.action(row, column);
    if (action && !action.disabled(row.object)) {
      action.execute(row.object);
    }
  }

  get dataAllorsActions() {
      return (this.table && this.table.actions) ? this.table.actions.map(v => v.name).join() : '';
  }

  private action(row: TableRow, column: Column) {
    return column.action || this.table.defaultAction;
  }

}
