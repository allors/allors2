import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Loaded, Scope, WorkspaceService } from '../../../../../angular';
import { InternalOrganisation, Request, RequestState } from '../../../../../domain';
import { And, ContainedIn, Equals, Like, Page, Predicate, PullRequest, Query, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

interface SearchData {
  requestNumber: string;
  company: string;
  description: string;
  state: string;
}

@Component({
  templateUrl: './requests-overview.component.html',
})
export class RequestsOverviewComponent implements OnInit, OnDestroy {
  public total: number;

  public searchForm: FormGroup;

  public title = 'Requests';
  public data: Request[];
  public filtered: Request[];

  public internalOrganisations: InternalOrganisation[];
  public selectedInternalOrganisation: InternalOrganisation;

  public requestStates: RequestState[];
  public selectedRequestState: RequestState;
  public requestState: RequestState;

  private refresh$: BehaviorSubject<Date>;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private router: Router,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      company: [''],
      description: [''],
      requestNumber: [''],
      state: [''],
    });
  }

  ngOnInit(): void {
    const search$ = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$ = Observable.combineLatest(search$, this.refresh$, this.stateService.internalOrganisationId$)
      .scan(([previousData, previousDate, previousInternalOrganisationId], [data, date, internalOrganisationId]) => {
        return [data, date, internalOrganisationId];
      }, [] as [SearchData, Date, InternalOrganisation]);

    const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

    this.subscription = combined$
      .switchMap(([data, , internalOrganisationId]) => {

        const internalOrganisationsQuery: Query[] = [
          new Query(
            {
              name: 'requestStates',
              objectType: m.RequestState,
            }),
          new Query(
            {
              name: 'internalOrganisations',
              objectType: m.Organisation,
              predicate: new Equals({ roleType: m.Organisation.IsInternalOrganisation, value: true }),
            }),
        ];

        return this.scope
        .load('Pull', new PullRequest({ queries: internalOrganisationsQuery }))
        .switchMap((loaded: Loaded) => {
          this.requestStates = loaded.collections.requestStates as RequestState[];
          this.requestState = this.requestStates.find((v: RequestState) => v.Name === data.state);

          this.internalOrganisations = loaded.collections.internalOrganisations as InternalOrganisation[];

          const predicate: And = new And();
          const predicates: Predicate[] = predicate.predicates;

          predicates.push(new Equals({ roleType: m.Request.Recipient, value: internalOrganisationId }));

          if (data.requestNumber) {
          const like: string = '%' + data.requestNumber + '%';
          predicates.push(new Like({ roleType: m.Request.RequestNumber, value: like }));
        }

          if (data.company) {
          const partyQuery: Query = new Query({
            objectType: m.Party, predicate: new Like({
              roleType: m.Party.PartyName, value: data.company.replace('*', '%') + '%',
            }),
          });

          const containedIn: ContainedIn = new ContainedIn({ roleType: m.Request.Originator, query: partyQuery });
          predicates.push(containedIn);
        }

          if (data.description) {
          const like: string = data.description.replace('*', '%') + '%';
          predicates.push(new Like({ roleType: m.Request.Description, value: like }));
        }

          if (data.state) {
          predicates.push(new Equals({ roleType: m.Request.RequestState, value: this.requestState }));
        }

          const queries: Query[] = [new Query(
          {
            include: [
              new TreeNode({ roleType: m.Request.Originator }),
              new TreeNode({ roleType: m.Request.RequestState }),
            ],
            name: 'requests',
            objectType: m.Request,
            predicate,
            sort: [new Sort({ roleType: m.Request.RequestNumber, direction: 'Desc' })],
          })];

          return this.scope.load('Pull', new PullRequest({ queries }));
        });
      })
      .subscribe((loaded) => {
        this.data = loaded.collections.requests as Request[];
        this.total = loaded.values.invoices_total;
      },
      (error: any) => {
        this.errorService.handle(error);
        this.goBack();
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goBack(): void {
    window.history.back();
  }

  public onView(request: Request): void {
    this.router.navigate(['/orders/requests/' + request.id + '/edit']);
  }
}
