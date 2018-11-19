import { MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';

import { TableConfig } from './TableConfig';
import { BaseTable } from './BaseTable';
import { Column } from './Column';
import { ActionTarget } from '../../actions';

export class Table<Row extends ActionTarget> extends BaseTable {

  dataSource: MatTableDataSource<Row>;
  selection: SelectionModel<Row>;

  constructor(config?: TableConfig) {
    super(config);

    this.dataSource = new MatTableDataSource();

    if (config) {

      this.columns = config.columns.map((v) => new Column(v));

      if (config.selection) {
        this.selection = new SelectionModel<Row>(true, []);
      }
    }
  }

  get data(): Row[] {
    return this.dataSource.data;
  }

  set data(value: Row[]) {
    this.dataSource.data = value;
  }
}
