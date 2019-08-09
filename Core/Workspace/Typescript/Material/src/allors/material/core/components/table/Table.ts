import { PageEvent } from '@angular/material/paginator';
import { Sort, MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';
import { BehaviorSubject } from 'rxjs';

import { TableConfig } from './TableConfig';
import { BaseTable } from './BaseTable';
import { Column } from './Column';
import { TableRow } from './TableRow';

export class Table<Row extends TableRow> extends BaseTable {

  dataSource: MatTableDataSource<Row>;
  selection: SelectionModel<Row>;

  private autoSort = false;

  constructor(config?: TableConfig) {
    super();

    let sort: Sort = null;

    if (config) {

      this.defaultAction = config.defaultAction;
      this.columns = config.columns.map((v) => new Column(v));

      if (config.selection) {
        this.selection = new SelectionModel<Row>(true, []);
      }

      if (config.initialSort) {
        if (typeof config.initialSort === 'string') {
          sort = {
            active: config.initialSort,
            direction: 'asc'
          };
        } else {
          sort = {
            active: config.initialSort.active,
            direction: config.initialSort.direction || 'asc'
          };
        }
      }

      this.pageSize = config.pageSize;
      this.pageSizeOptions = config.pageSizeOptions;

      if (!this.pageSize && !!this.pageSizeOptions && this.pageSizeOptions.length > 0) {
        this.pageSize = this.pageSizeOptions[0];
      }

      if (!!this.pageSize && !this.pageSizeOptions) {
        this.pageSizeOptions = [this.pageSize, this.pageSize * 2, this.pageSize * 5];
      }

      this.autoSort = config.autoSort;
      this.autoFilter = config.autoFilter;
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

  Init(sort: MatSort) {
    if (this.autoSort) {
      this.dataSource.sort = sort;
    }
  }
}
