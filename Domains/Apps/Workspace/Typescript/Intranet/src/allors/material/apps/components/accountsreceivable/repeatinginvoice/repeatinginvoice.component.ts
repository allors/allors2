import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';


import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Field, FilterFactory, Loaded, Saved, Scope, WorkspaceService } from '../../../../../angular';
import { DayOfWeek, IncoTermType, RepeatingSalesInvoice, SalesInvoice, SalesTerm, TimeFrequency } from '../../../../../domain';
import { Fetch, Path, PullRequest, Query, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { DialogService } from '../../../../base/services/dialog';

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
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: DialogService) {

    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$)
      .switchMap(([urlSegments, date]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const repeatingInvoiceId: string = this.route.snapshot.paramMap.get('repeatingInvoiceId');
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          new Fetch({
            id,
            name: 'salesInvoice',
          }),
          new Fetch({
            id: repeatingInvoiceId,
            include: [
              new TreeNode({ roleType: this.m.RepeatingSalesInvoice.Frequency }),
              new TreeNode({ roleType: this.m.RepeatingSalesInvoice.DayOfWeek }),
          ],
            name: 'repeatingInvoice',
          }),
        ];

        const queries: Query[] = [
          new Query({
            name: 'frequencies',
            objectType: m.TimeFrequency,
          }),
          new Query({
            name: 'daysOfWeek',
            objectType: m.DayOfWeek,
          }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }));
      })
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
