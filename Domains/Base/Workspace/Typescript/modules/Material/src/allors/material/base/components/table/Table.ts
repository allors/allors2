import { MatTableDataSource, Sort, PageEvent } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { BehaviorSubject } from 'rxjs';

import { TableConfig } from './TableConfig';
import { BaseTable } from './BaseTable';
import { Column } from './Column';
import { TableRow } from './TableRow';

export class Table<Row extends TableRow> extends BaseTable {

  dataSource: MatTableDataSource<Row>;
  selection: SelectionModel<Row>;

  constructor(config?: TableConfig) {
    super();

    let sort: Sort;

    if (config) {

      this.defaultAction = config.defaultAction;
      this.columns = config.columns.map((v) => new Column(v));

      if (config.selection) {
        this.selection = new SelectionModel<Row>(true, []);
      }

      const column = this.columns && this.columns.find(v => v.sort);
      sort = column && { active: column.name, direction: 'asc' };

      const initialSort = config.sort;
      if (sort && initialSort) {
        if (initialSort.active) {
          sort.active = initialSort.active;
        }

        if (initialSort.direction) {
          sort.direction = initialSort.direction;
        }
      }
    }

    this.dataSource = new MatTableDataSource();
    this.actions = (config && config.actions) || [];
    this.pager$ = new BehaviorSubject<PageEvent>(Object.assign(new PageEvent(), { pageIndex: 0, pageSize: 50 }));

    this.sort$ = new BehaviorSubject<Sort>(sort);
  }

  get data(): Row[] {
    return this.dataSource.data;
  }

  set data(value: Row[]) {
    this.dataSource.data = value;
  }
}
