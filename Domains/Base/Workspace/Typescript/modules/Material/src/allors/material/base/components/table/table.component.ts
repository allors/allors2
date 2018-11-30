import { Component, Input } from '@angular/core';

import { BaseTable } from './BaseTable';
import { Column } from './Column';
import { TableRow } from './TableRow';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss']
})
export class AllorsMaterialTableComponent {
  @Input()
  public table: BaseTable;

  cellStyle(row: TableRow, column: Column): string {
    return  this.action(row, column) ? 'pointer' : undefined;
  }

  onCellClick(row: TableRow, column: Column) {

    const action = this.action(row, column);
    if (action) {
      action.execute(row.object);
    }
  }

  private action(row: TableRow, column: Column) {
    return column.action || this.table.defaultAction;
  }
}
