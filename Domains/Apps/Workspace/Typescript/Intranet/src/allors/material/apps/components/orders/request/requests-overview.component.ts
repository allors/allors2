import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

import { ErrorService, Loaded, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { InternalOrganisation, Request, RequestState } from '../../../../../domain';
import { And, ContainedIn, Equals, Like, Predicate, PullRequest, Sort, TreeNode, Filter } from '../../../../../framework';
import { StateService } from '../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

interface SearchData {
  requestNumber: string;
  company: string;
  description: string;
  state: string;
}

@Component({
  templateUrl: './requests-overview.component.html'
})
export class RequestsOverviewComponent implements OnInit, OnDestroy {
  public total: number;

  public searchForm: FormGroup;
  public advancedSearch: boolean;

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
    private dataService: DataService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private router: Router,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService
  ) {
    titleService.setTitle(this.title);

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      company: [''],
      description: [''],
      requestNumber: [''],
      state: ['']
    });
  }

  ngOnInit(): void {

    const { m, pull } = this.dataService;

    const search$ = this.searchForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({})
      );

    const combined$ = combineLatest(search$, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousData, previousDate, previousInternalOrganisationId], [data, date, internalOrganisationId]) => {
          return [data, date, internalOrganisationId];
        }, [])
      );

    this.subscription = combined$
      .pipe(
        switchMap(([data, , internalOrganisationId]) => {
          const pulls = [
            pull.RequestState({
              sort: new Sort(m.RequestState.Name)
            }),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: new Sort(m.Organisation.PartyName)
            })
          ];

          return this.scope
            .load(
              'Pull',
              new PullRequest({ pulls })
            )
            .pipe(
              switchMap((loaded: Loaded) => {
                this.requestStates = loaded.collections
                  .requestStates as RequestState[];
                this.requestState = this.requestStates.find(
                  (v: RequestState) => v.Name === data.state
                );

                this.internalOrganisations = loaded.collections
                  .internalOrganisations as InternalOrganisation[];

                const predicate: And = new And();
                const predicates: Predicate[] = predicate.operands;

                predicates.push(
                  new Equals({ propertyType: m.Request.Recipient, value: internalOrganisationId })
                );

                if (data.requestNumber) {
                  const like: string = '%' + data.requestNumber + '%';
                  predicates.push(
                    new Like({ roleType: m.Request.RequestNumber, value: like })
                  );
                }

                if (data.company) {
                  const partyQuery = new Filter({
                    objectType: m.Party,
                    predicate: new Like({
                      roleType: m.Party.PartyName,
                      value: data.company.replace('*', '%') + '%'
                    })
                  });

                  const containedIn: ContainedIn = new ContainedIn({
                    propertyType: m.Request.Originator,
                    extent: partyQuery
                  });
                  predicates.push(containedIn);
                }

                if (data.description) {
                  const like: string = data.description.replace('*', '%') + '%';
                  predicates.push(
                    new Like({ roleType: m.Request.Description, value: like })
                  );
                }

                if (data.state) {
                  predicates.push(
                    new Equals({propertyType: m.Request.RequestState, object: this.requestState})
                  );
                }

                const queries = [
                  pull.Request(
                    {
                      predicate,
                      include: {
                        Originator: x,
                        RequestState: x,
                      },
                      sort: new Sort(m.Request.RequestNumber)
                    }
                  )
                ];

                return this.scope.load('Pull', new PullRequest({ pulls: queries }));
              })
            );
        })
      )
      .subscribe(
        loaded => {
          this.data = loaded.collections.requests as Request[];
          this.total = loaded.values.invoices_total;
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

  public goBack(): void {
    window.history.back();
  }

  public onView(request: Request): void {
    this.router.navigate(['/orders/requests/' + request.id + '/edit']);
  }
}
