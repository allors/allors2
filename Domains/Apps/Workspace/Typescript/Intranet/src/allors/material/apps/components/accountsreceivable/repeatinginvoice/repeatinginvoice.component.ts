import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';


import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Field, SearchFactory, Loaded, Saved, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { DayOfWeek, IncoTermType, RepeatingSalesInvoice, SalesInvoice, SalesTerm, TimeFrequency } from '../../../../../domain';
import { PullRequest, Fetch, Sort, TreeNode, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './repeatinginvoice.component.html',
})
export class RepeatingInvoiceEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title = 'Edit Repeating Invoice';
  public subTitle: string;
  public invoice: SalesInvoice;
  public repeatinginvoice: RepeatingSalesInvoice;
  public frequencies: TimeFrequency[];
  public daysOfWeek: DayOfWeek[];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService) {

    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { pull } = this.dataService;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const repeatingInvoiceId: string = this.route.snapshot.paramMap.get('repeatingInvoiceId');
          const m: MetaDomain = this.m;

          const pulls = [

            pull.SalesInvoice({object: id}),
            pull.RepeatingSalesInvoice({
              object: id,
              include: {
                Frequency: x,
                DayOfWeek: x,
              }
            }),
            pull.TimeFrequency({
              predicate: new Equals({ propertyType: m.TimeFrequency.IsActive, value: true }),
              sort: new Sort(m.TimeFrequency.Name),
            }),
            pull.DayOfWeek()
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.invoice = loaded.objects.salesInvoice as SalesInvoice;
        this.repeatinginvoice = loaded.objects.repeatingInvoice as RepeatingSalesInvoice;
        this.frequencies = loaded.collections.frequencies as TimeFrequency[];
        this.daysOfWeek = loaded.collections.daysOfWeek as DayOfWeek[];

        if (!this.repeatinginvoice) {
          this.title = 'Add Repeating Invoice';
          this.repeatinginvoice = this.scope.session.create('RepeatingSalesInvoice') as RepeatingSalesInvoice;
          this.repeatinginvoice.Source = this.invoice;
        }
      },
        (error: Error) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(['/accountsreceivable/invoice/' + this.invoice.id]);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
