import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Loaded, PdfService, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { InternalOrganisation, SalesOrder, SalesOrderState } from '../../../../../domain';
import { And, ContainedIn, Equals, Like, Predicate, PullRequest, Sort, TreeNode, Filter } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { debounceTime, distinctUntilChanged, startWith, switchMap, scan } from 'rxjs/operators';

interface SearchData {
  internalOrganisation: string;
  company: string;
  reference: string;
  orderNumber: string;
  state: string;
}

@Component({
  templateUrl: './salesorders-overview.component.html'
})
export class SalesOrdersOverviewComponent implements OnInit, OnDestroy {
  public searchForm: FormGroup;
  public advancedSearch: boolean;

  public title = 'Sales Orders';
  public data: SalesOrder[];
  public filtered: SalesOrder[];
  public total: number;

  public internalOrganisations: InternalOrganisation[];
  public selectedInternalOrganisation: InternalOrganisation;
  public billToInternalOrganisation: InternalOrganisation;

  public orderStates: SalesOrderState[];
  public selectedOrderState: SalesOrderState;
  public orderState: SalesOrderState;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private router: Router,
    private dialogService: AllorsMaterialDialogService,
    public pdfService: PdfService,
    private stateService: StateService
  ) {
    titleService.setTitle(this.title);

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      internalOrganisation: [''],
      company: [''],
      orderNumber: [''],
      reference: [''],
      state: ['']
    });
  }

  ngOnInit(): void {

    const { m, pull } = this.dataService;

    const search$ = this.searchForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({}),
      );

    const combined$ = combineLatest(search$, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(
          ([previousData, previousDate, previousInternalOrganisationId], [data, date, internalOrganisationId]) => {
            return [data, date, internalOrganisationId];
          }, [])
      );

    this.subscription = combined$
      .pipe(
        switchMap(([data, , internalOrganisationId]) => {

          const pulls = [
            pull.SalesOrderState({
              sort: new Sort(m.SalesOrderState.Name)
            }),
            pull.SalesOrderState(),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: new Sort(m.Organisation.PartyName)
            })
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded: Loaded) => {
                this.orderStates = loaded.collections.orderStates as SalesOrderState[];
                this.orderState = this.orderStates.find(
                  (v: SalesOrderState) => v.Name === data.state
                );

                this.internalOrganisations = loaded.collections
                  .internalOrganisations as InternalOrganisation[];
                this.billToInternalOrganisation = this.internalOrganisations.find(
                  v => v.PartyName === data.internalOrganisation
                );

                const predicate: And = new And();
                const predicates: Predicate[] = predicate.operands;

                predicates.push(
                  new Equals({ propertyType: m.SalesOrder.TakenBy, value: internalOrganisationId })
                );

                if (data.orderNumber) {
                  const like: string = '%' + data.orderNumber + '%';
                  predicates.push(
                    new Like({ roleType: m.SalesOrder.OrderNumber, value: like })
                  );
                }

                if (data.company) {
                  predicates.push(
                    new ContainedIn({
                      propertyType: m.SalesOrder.ShipToCustomer,
                      extent: new Filter({
                        objectType: m.Party,
                        predicate: new Like({ roleType: m.Party.PartyName, value: data.company.replace('*', '%') + '%' })
                      })
                    })
                  );
                }

                if (data.internalOrganisation) {
                  predicates.push(
                    new Equals({ propertyType: m.SalesOrder.BillToCustomer, object: this.billToInternalOrganisation })
                  );
                }

                if (data.reference) {
                  const like: string = data.reference.replace('*', '%') + '%';
                  predicates.push(
                    new Like({ roleType: m.SalesOrder.CustomerReference, value: like })
                  );
                }

                if (data.state) {
                  predicates.push(
                    new Equals({ propertyType: m.SalesOrder.SalesOrderState, object: this.orderState })
                  );
                }

                const pulls2 = [
                  pull.SalesOrder({
                    predicate,
                    include:
                    {
                      ShipToCustomer: x,
                      SalesOrderState: x,
                    },
                    sort: new Sort(m.SalesOrder.OrderNumber)
                  })
                ];

                return this.scope.load('Pull', new PullRequest({ pulls: pulls2 }));
              })
            );
        })
      )
      .subscribe(
        loaded => {
          this.data = loaded.collections.orders as SalesOrder[];
          this.total = loaded.values.orders_total;
        },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        }
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public print(order: SalesOrder) {
    this.pdfService.display(order);
  }

  public goBack(): void {
    window.history.back();
  }

  public onView(order: SalesOrder): void {
    this.router.navigate(['/orders/salesOrders/' + order.id]);
  }
}
