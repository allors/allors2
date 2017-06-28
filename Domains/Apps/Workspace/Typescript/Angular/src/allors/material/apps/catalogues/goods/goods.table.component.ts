import { Component, OnChanges, SimpleChanges, Input, Output, EventEmitter } from '@angular/core';
import { TdDataTableService, TdDataTableSortingOrder, ITdDataTableSortChangeEvent, ITdDataTableRowClickEvent, ITdDataTableColumn, IPageChangeEvent } from '@covalent/core';

import { Good } from '../../../../domain';

@Component({
  selector: 'goods-table',
  template: `
<ng-template tdLoading="objects">
  <md-list class="will-load">

  <div class="md-padding">


<table td-data-table>
  <th td-data-table-column>
    Photo
  </th>
  <th td-data-table-column
      *ngFor="let column of columns"
      [numeric]="column.numeric">
    {{column.label}}
  </th>
  <th td-data-table-column>
  </th>
  <tr td-data-table-row *ngFor="let row of filteredData">
  <td>
    <md-icon *ngIf="!row['PrimaryPhoto']" md-list-avatar>photo_camera</md-icon>
    <img *ngIf="row['PrimaryPhoto']" md-list-avatar src="http://localhost:5000/Media/Download/{{row['PrimaryPhoto'].UniqueId}}?revision={{row['PrimaryPhoto'].Revision}}"
    width="90" height="60" />
  </td>
    <td td-data-table-cell *ngFor="let column of columns" [numeric]="column.numeric">
      {{column.format ? column.format(row[column.name]) : row[column.name]}}
    </td>
    <td td-data-table-cell>
      <button md-button (click)="onView(row)">Edit</button>
    </td>
  </tr>
</table>

    <td-paging-bar #pagingBar [pageSizes]="[5, 10, 15, 20]" [total]="filteredTotal" (change)="page($event)">
      <span td-paging-bar-label hide-xs>Row per page:</span> {{pagingBar.range}} <span hide-xs>of {{pagingBar.total}}</span>
    </td-paging-bar>
  </div>

  </md-list>
</ng-template>
`,
})
export class GoodsTableComponent implements OnChanges {

  columns: ITdDataTableColumn[] = [
    // { name: 'PrimaryPhoto', label: 'Photo', sortable: false },
    { name: 'Name', label: 'Name', sortable: true },
    { name: 'Description', label: 'Description', sortable: true },
    { name: 'Primary category', label: 'PrimaryCategory', sortable: true },
    { name: 'ArticleNumber', label: 'Article number', sortable: true },
  ];

  @Input()
  data: Good[];

  @Output()
  view: EventEmitter<Good> = new EventEmitter();

  fromRow: number = 1;
  currentPage: number = 1;
  pageSize: number = 5;
  sortBy: string = 'Name';
  sortOrder: TdDataTableSortingOrder = TdDataTableSortingOrder.Descending;

  filteredData: Good[];
  filteredTotal: number;

  constructor(private _dataTableService: TdDataTableService) {
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.filter();
  }

  rowClicked(selectEvent: ITdDataTableRowClickEvent): void {
    console.debug(selectEvent);
  }

  sort(sortEvent: ITdDataTableSortChangeEvent): void {
    this.sortBy = sortEvent.name;
    this.sortOrder = sortEvent.order;
    this.filter();
  }

  page(pagingEvent: IPageChangeEvent): void {
    this.fromRow = pagingEvent.fromRow;
    this.currentPage = pagingEvent.page;
    this.pageSize = pagingEvent.pageSize;
    this.filter();
  }

  filter(): void {
    let newData: any[] = this.data ? this.data.map((v: any) => v) : [];
    this.filteredTotal = newData.length;
    newData = this._dataTableService.sortData(newData, this.sortBy, this.sortOrder);
    newData = this._dataTableService.pageData(newData, this.fromRow, this.currentPage * this.pageSize);
    this.filteredData = newData;
  }

  onView(good: Good): void {
    this.view.emit(good);
  }
}
