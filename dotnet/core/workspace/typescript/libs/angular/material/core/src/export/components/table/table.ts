import { PageEvent } from '@angular/material/paginator';
import { Sort, MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';
import { BehaviorSubject } from 'rxjs';

import { Action } from '@allors/angular/core';
import { ISessionObject } from '@allors/domain/system';

import { TableConfig } from './TableConfig';
import { BaseTable } from './BaseTable';
import { Column } from './Column';
import { TableRow } from './TableRow';

export class Table<Row extends TableRow> implements BaseTable {

  dataSource: MatTableDataSource<Row>;
  selection: SelectionModel<Row>;

  columns: Column[];
  actions: Action[];
  defaultAction?: Action;

  pageIndex: number;
  pageSize?: number;
  pageSizeOptions?: number[];

  sort$: BehaviorSubject<Sort | null>;
  pager$: BehaviorSubject<PageEvent>;

  total: number;

  autoFilter: boolean;

  private autoSort = false;

  constructor(config?: TableConfig) {

    let sort: Sort | null = null;

    if (config) {

      this.defaultAction = config.defaultAction;
      this.columns = config.columns?.map((v) => new Column(v)) ?? [];

      if (config.selection) {
        this.selection = new SelectionModel<Row>(true, []);
      }

      if (config.initialSort) {
        if (typeof config.initialSort === 'string') {
          sort = {
            active: config.initialSort,
            direction: config.initialSortDirection || 'asc'
          };
        } else if (config.initialSort.active) {
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

      this.autoSort = config.autoSort ?? false;
      this.autoFilter = config.autoFilter ?? false;
    }

    this.dataSource = new MatTableDataSource();
    this.actions = (config && config.actions) || [];
    this.pager$ = new BehaviorSubject<PageEvent>(Object.assign(new PageEvent(), { pageIndex: 0, pageSize: 50 }));

    this.sort$ = new BehaviorSubject<Sort | null>(sort);
  }

  get sortValue(): Sort | null {
    return this.sort$.getValue();
  }

  get hasActions(): boolean {
    return this.actions && this.actions.length > 0;
  }

  get columnNames(): string[] {
    let result = this.columns.map((v) => v.name);
    if (this.selection) {
      result = ['select', ...result];
    }

    if (this.hasActions) {
      result = [...result, 'menu'];
    }

    return result;
  }

  get anySelected() {
    return !this.selection.isEmpty();
  }

  get allSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  get selected(): ISessionObject[] {
    return this.selection.selected.map((v => v.object));
  }

  masterToggle() {
    this.allSelected ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

  page(event: PageEvent): void {
    this.pager$.next(event);
  }

  sort(event: Sort): void {
    this.sort$?.next(event);
  }

  filter(event: any): void {
    const value = event && event.target && event.target.value;
    this.dataSource.filter = value;
  }

  get data(): Row[] {
    return this.dataSource.data;
  }

  set data(value: Row[]) {
    if (this.selection) {
      this.selection.clear();
    }
    this.dataSource.data = value;
    if (this.pageSize && this.pageSizeOptions && this.total > Math.max(...this.pageSizeOptions)) {
      this.pageSizeOptions = [this.pageSize, this.pageSize * 2, this.pageSize * 5, this.total * 1];
    }
  }

  init(sort: MatSort) {
    if (this.autoSort) {
      this.dataSource.sort = sort;
    }
  }
}
