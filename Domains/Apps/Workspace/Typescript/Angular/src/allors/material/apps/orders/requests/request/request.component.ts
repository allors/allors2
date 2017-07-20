import { Observable, BehaviorSubject, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService, TdDialogService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta';
import { PullRequest, PushResponse, Contains, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import {
  Currency, Organisation, Party, Person, PartyContactMechanism, ContactMechanism,
  OrganisationRole, PersonRole, RequestForQuote,
} from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Filter, Invoked } from '../../../../../angular';

@Component({
  templateUrl: './request.component.html',
})
export class RequestFormComponent implements OnInit, AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  flex: string = '100%';
  flex2: string = `calc(50%-25px)`;

  m: MetaDomain;

  title: string;
  request: RequestForQuote;
  people: Person[];
  organisations: Organisation[];
  currencies: Currency[];
  contactMechanisms: ContactMechanism[];

  peopleFilter: Filter;
  organisationsFilter: Filter;
  currenciesFilter: Filter;

  get showOrganisations(): boolean {
    return !this.request.Originator || this.request.Originator instanceof (Organisation);
  }
  get showPeople(): boolean {
    return !this.request.Originator || this.request.Originator instanceof (Person);
  }

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MdSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService) {

    this.scope = new Scope(allorsService.database, allorsService.workspace);
    this.m = this.allorsService.meta;
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.peopleFilter = new Filter(this.scope, this.m.Person, [this.m.Person.FirstName, this.m.Person.LastName]);
    this.organisationsFilter = new Filter(this.scope, this.m.Organisation, [this.m.Organisation.Name]);
    this.currenciesFilter = new Filter(this.scope, this.m.Currency, [this.m.Currency.Name]);
  }

  ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'requestForQuote',
            id: id,
            include: [
              new TreeNode({ roleType: m.Request.Originator }),
            ],
          }),
        ];

        const rolesQuery: Query[] = [
          new Query(
            {
              name: 'organisationRoles',
              objectType: m.OrganisationRole,
            }),
          new Query(
            {
              name: 'personRoles',
              objectType: m.PersonRole,
            }),
          new Query(
            {
              name: 'currencies',
              objectType: this.m.Currency,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ query: rolesQuery }))
          .switchMap((loaded: Loaded) => {
            this.currencies = loaded.collections.currencies as Currency[];

            const organisationRoles: OrganisationRole[] = loaded.collections.organisationRoles as OrganisationRole[];
            const oCustomerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === 'Customer');

            const personRoles: OrganisationRole[] = loaded.collections.organisationRoles as OrganisationRole[];
            const pCustomerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === 'Customer');

            const query: Query[] = [
              new Query(
                {
                  name: 'organisations',
                  predicate: new Contains({ roleType: m.Organisation.OrganisationRoles, object: oCustomerRole }),
                  objectType: this.m.Organisation,
                }),
              new Query(
                {
                  name: 'persons',
                  predicate: new Contains({ roleType: m.Person.PersonRoles, object: pCustomerRole }),
                  objectType: this.m.Person,
                }),
            ];

            return this.scope.load('Pull', new PullRequest({ fetch: fetch, query: query }));
          });
      })
      .subscribe((loaded: Loaded) => {

        this.request = loaded.objects.requestForQuote as RequestForQuote;
        if (!this.request) {
          this.request = this.scope.session.create('RequestForQuote') as RequestForQuote;
        }

        this.organisations = loaded.collections.organisations as Organisation[];
        this.people = loaded.collections.parties as Person[];
        this.title = 'Request from: ' + this.request.Originator.PartyName;
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  submit(): void {
    const submitFn: () => void = () => {
      this.scope.invoke(this.request.Submit)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully submitted.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: 'Save changes?' })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                submitFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            submitFn();
          }
        });
    } else {
      submitFn();
    }
  }

  complete(): void {
    const completeFn: () => void = () => {
      this.scope.invoke(this.request.Complete)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully completed.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: 'Save changes?' })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                completeFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            completeFn();
          }
        });
    } else {
      completeFn();
    }
  }

  cancel(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.request.Cancel)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: 'Save changes?' })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  hold(): void {
    const holdFn: () => void = () => {
      this.scope.invoke(this.request.Hold)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully held.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: 'Save changes?' })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                holdFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            holdFn();
          }
        });
    } else {
      holdFn();
    }
  }

  reject(): void {
    const rejectFn: () => void = () => {
      this.scope.invoke(this.request.Reject)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: 'Save changes?' })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                rejectFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            rejectFn();
          }
        });
    } else {
      rejectFn();
    }
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(['/orders/requests/' + this.request.id + '/overview']);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  originatorSelected(party: Party): void {

    const fetch: Fetch[] = [
      new Fetch({
        name: 'partyContactMechanisms',
        id: party.id,
        path: new Path({ step: this.m.Party.CurrentPartyContactMechanisms }),
        include: [
          new TreeNode({
            roleType: this.m.PartyContactMechanism.ContactMechanism,
            nodes: [
              new TreeNode({
                roleType: this.m.PostalAddress.PostalBoundary,
                nodes: [
                  new TreeNode({ roleType: this.m.PostalBoundary.Country }),
                ],
              }),
            ],
          }),
        ],
      }),
    ];

    this.scope
      .load('Pull', new PullRequest({ fetch: fetch }))
      .subscribe((loaded: Loaded) => {

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.contactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  refresh(): void {
    this.refresh$.next(new Date());
  }

  goBack(): void {
    window.history.back();
  }
}
