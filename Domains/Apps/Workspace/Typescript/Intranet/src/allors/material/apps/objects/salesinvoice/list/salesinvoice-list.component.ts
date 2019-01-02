import { Component, OnDestroy, Self, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource, PageEvent, MatSnackBar } from '@angular/material';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { scan, switchMap } from 'rxjs/operators';

import { AllorsFilterService, ErrorService, ContextService, NavigationService, MediaService, MetaService, RefreshService } from '../../../../../angular';
import { SalesInvoice } from '../../../../../domain';
import { And, Like, PullRequest, Sort } from '../../../../../framework';
import { PrintService, Sorter } from '../../../../../material';

interface Row {
  salesInvoice: SalesInvoice;
  number: string;
  billedTo: string;
  reference: string;
  description: string;
  lastModifiedDate: Date;
}

@Component({
  templateUrl: './salesinvoice-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class SalesInvoiceListComponent implements OnInit, OnDestroy {

  public searchForm: FormGroup; public advancedSearch: boolean;

  public title = 'Sales Invoices';

  public displayedColumns = ['select', 'number', 'billedTo', 'reference', 'description', 'lastModifiedDate', 'menu'];
  public selection = new SelectionModel<Row>(true, []);

  public total: number;
  public dataSource = new MatTableDataSource<Row>();

  private sort$: BehaviorSubject<Sort>;
  private pager$: BehaviorSubject<PageEvent>;
  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public printService: PrintService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.sort$ = new BehaviorSubject<Sort>(undefined);
    this.pager$ = new BehaviorSubject<PageEvent>(Object.assign(new PageEvent(), { pageIndex: 0, pageSize: 50 }));
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Like({ roleType: m.SalesInvoice.InvoiceNumber, parameter: 'number' }),
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        number: m.SalesInvoice.InvoiceNumber,
        lastModifiedDate: m.Person.LastModifiedDate,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.sort$, this.pager$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
          ];
        }, []),
        switchMap(([, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.SalesInvoice({
              predicate,
              sort: sorter.create(sort),
              include: {
                PrintDocument: x,
                BillToCustomer: x,
                SalesInvoiceState: x,
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();
        this.total = loaded.values.People_total;
        const salesInvoices = loaded.collections.SalesInvoices as SalesInvoice[];

        this.dataSource.data = salesInvoices.map((v) => {
          return {
            salesInvoice: v,
            number: v.InvoiceNumber,
            billedTo: v.BillToCustomer.displayName,
            reference: `${v.CustomerReference} - ${v.SalesInvoiceState.Name}`,
            description: v.Description,
            lastModifiedDate: v.LastModifiedDate,
          } as Row;
        });
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public get hasSelection() {
    return !this.selection.isEmpty();
  }

  public get selectedPeople() {
    return this.selection.selected.map(v => v.salesInvoice);
  }

  public isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  public masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

  public sort(event: Sort): void {
    this.sort$.next(event);
  }

  public page(event: PageEvent): void {
    this.pager$.next(event);
  }
}
