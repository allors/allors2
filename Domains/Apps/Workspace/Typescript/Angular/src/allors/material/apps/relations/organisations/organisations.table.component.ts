import { Component, OnChanges, SimpleChanges, Input, Output, EventEmitter } from '@angular/core';
import { TdDataTableService, TdDataTableSortingOrder, ITdDataTableSortChangeEvent, ITdDataTableRowClickEvent, ITdDataTableColumn, IPageChangeEvent } from '@covalent/core';

import { Organisation } from '../../../../domain';

@Component({
  selector: 'organisations-table',
  template: `
<ng-template tdLoading="objects">
  <md-list class="will-load">

  <div class="md-padding">


<table td-data-table>
  <th td-data-table-column
      *ngFor="let column of columns"
      [numeric]="column.numeric">
    {{column.label}}
  </th>
  <th td-data-table-column>
  </th>
  <tr td-data-table-row *ngFor="let row of filteredData">
    <td td-data-table-cell *ngFor="let column of columns" [numeric]="column.numeric">
      {{column.format ? column.format(row[column.name]) : row[column.name]}}
    </td>
    <td td-data-table-cell>
      <button md-button (click)="onView(row)">Details</button>
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
export class OrganisationsTableComponent implements OnChanges {

  columns: ITdDataTableColumn[] = [
    { name: 'Name', label: 'Name', sortable: true },
    { name: 'GeneralCorrespondence.Address1', label: 'Address1', sortable: true },
    { name: 'GeneralCorrespondence.Address2', label: 'Address2', sortable: true },
    { name: 'GeneralCorrespondence.Address3', label: 'Address3', sortable: true },
    { name: 'GeneralCorrespondence?.PostalBoundary?.PostalCode', label: 'Postal Code', sortable: true },
    { name: 'GeneralCorrespondence?.PostalBoundary?.Locality', label: 'Locality', sortable: true },
    { name: 'GeneralCorrespondence?.PostalBoundary?.Country.Name', label: 'Country', sortable: true },
  ];

  @Input()
  data: Organisation[];

  @Output()
  view: EventEmitter<Organisation> = new EventEmitter();

  fromRow: number = 1;
  currentPage: number = 1;
  pageSize: number = 5;
  sortBy: string = 'Name';
  sortOrder: TdDataTableSortingOrder = TdDataTableSortingOrder.Descending;

  filteredData: Organisation[];
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
    let newData: any[] = this.data ? this.data.map(v => v) : [];
    this.filteredTotal = newData.length;
    newData = this._dataTableService.sortData(newData, this.sortBy, this.sortOrder);
    newData = this._dataTableService.pageData(newData, this.fromRow, this.currentPage * this.pageSize);
    this.filteredData = newData;
  }

  onView (organisation: Organisation) {
    this.view.emit(organisation);
  }
}
