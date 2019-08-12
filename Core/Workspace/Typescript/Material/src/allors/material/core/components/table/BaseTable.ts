// tslint:disable: directive-selector
// tslint:disable: directive-class-suffix
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';

import { Action } from '../../../../angular';

import { Column } from './Column';
import { BehaviorSubject } from 'rxjs';
import { TableRow } from './TableRow';
import { ISessionObject } from '../../../../../allors/framework';
import { Directive } from '@angular/core';

// See https://github.com/angular/angular/issues/30080
@Directive({selector: 'ivy-workaround-base-table'})
export abstract class BaseTable {
  columns: Column[];
  dataSource: MatTableDataSource<TableRow>;
  selection: SelectionModel<TableRow>;
  actions: Action[];
  defaultAction: Action;
  pageSize: number;
  pageSizeOptions: number[];

  sort$: BehaviorSubject<Sort>;
  pager$: BehaviorSubject<PageEvent>;

  total: number;

  autoFilter: boolean;

  get sortValue(): Sort {
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

  abstract Init(matSort: any);

  masterToggle() {
    this.allSelected ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

  page(event: PageEvent): void {
    this.pager$.next(event);
  }

  sort(event: Sort): void {
    this.sort$.next(event);
  }

  filter(event: any): void {
    const value = event && event.target && event.target.value;
    this.dataSource.filter = value;
  }
}
