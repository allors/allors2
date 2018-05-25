import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Field, FilterFactory, Invoked, Loaded, Saved, Scope, WorkspaceService } from '../../../../../angular';
import { ContactMechanism, Currency, InternalOrganisation, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, RequestForQuote } from '../../../../../domain';
import { Contains, Equals, Fetch, Path, PullRequest, Query, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { Fetcher } from '../../Fetcher';
import { DialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './request.component.html',
})
export class RequestEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title: string;
  public subTitle: string;
  public request: RequestForQuote;
  public currencies: Currency[];
  public contactMechanisms: ContactMechanism[];
  public contacts: Person[];
  public scope: Scope;

  public addContactPerson = false;
  public addContactMechanism = false;

  private subscription: Subscription;
  private previousOriginator: Party;

  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;

  get showOrganisations(): boolean {
    return !this.request.Originator || this.request.Originator.objectType.name === 'Organisation';
  }
  get showPeople(): boolean {
    return !this.request.Originator || this.request.Originator.objectType.name === 'Person';
  }

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: DialogService,
    public stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;

    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.m);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([urlSegments, date, internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const rolesQuery: Query[] = [
          new Query(m.PersonRole),
          new Query(m.Currency),
        ];

        return this.scope
          .load('Pull', new PullRequest({ queries: rolesQuery }))
          .switchMap((loaded) => {
            this.scope.session.reset();
            this.currencies = loaded.collections.Currencies as Currency[];

            const fetches: Fetch[] = [
              this.fetcher.internalOrganisation,
              new Fetch({
                id,
                include: [
                  new TreeNode({ roleType: m.Request.Currency }),
                  new TreeNode({ roleType: m.Request.Originator }),
                  new TreeNode({ roleType: m.Request.RequestState }),
                  new TreeNode({
                    nodes: [
                      new TreeNode({
                        nodes: [
                          new TreeNode({ roleType: m.PostalBoundary.Country }),
                        ],
                        roleType: m.PostalAddress.PostalBoundary,
                      }),
                    ],
                    roleType: m.Request.FullfillContactMechanism,
                  })],
                name: 'requestForQuote',
              }),
            ];

            return this.scope.load('Pull', new PullRequest({ fetches }));
          });
      })
      .subscribe((loaded) => {

        this.request = loaded.objects.requestForQuote as RequestForQuote;
        const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;

        if (!this.request) {
          this.request = this.scope.session.create('RequestForQuote') as RequestForQuote;
          this.request.Recipient = internalOrganisation;
          this.request.RequestDate = new Date();
          this.title = 'Add Request';
        } else {
          this.title = 'Request ' + this.request.RequestNumber;
        }

        if (this.request.Originator) {
          this.update(this.request.Originator);
          this.title = this.title + ' from: ' + this.request.Originator.PartyName;
        }

        this.previousOriginator = this.request.Originator;
      },
      (error: Error) => {
        this.errorService.handle(error);
        this.goBack();
      },
    );
  }

  public submit(): void {
    const submitFn: () => void = () => {
      this.scope.invoke(this.request.Submit)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully submitted.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.handle(error);
        });
    };

    if (this.scope.session.hasChanges) {
      // TODO:
      /*  this.dialogService
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
                this.errorService.handle(error);
              });
          } else {
            submitFn();
          }
        }); */
    } else {
      submitFn();
    }
  }

  public cancel(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.request.Cancel)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.handle(error);
        });
    };

    if (this.scope.session.hasChanges) {
      // TODO:
      /* this.dialogService
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
                this.errorService.handle(error);
              });
          } else {
            cancelFn();
          }
        }); */
    } else {
      cancelFn();
    }
  }

  public hold(): void {
    const holdFn: () => void = () => {
      this.scope.invoke(this.request.Hold)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully held.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.handle(error);
        });
    };

    if (this.scope.session.hasChanges) {
      // TODO:
      /*  this.dialogService
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
                this.errorService.handle(error);
              });
          } else {
            holdFn();
          }
        }); */
    } else {
      holdFn();
    }
  }

  public reject(): void {
    const rejectFn: () => void = () => {
      this.scope.invoke(this.request.Reject)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.handle(error);
        });
    };

    if (this.scope.session.hasChanges) {
      // TODO:
     /*  this.dialogService
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
                this.errorService.handle(error);
              });
          } else {
            rejectFn();
          }
        }); */
    } else {
      rejectFn();
    }
  }

  public personCancelled(): void {
    this.addContactPerson = false;
  }

  public personAdded(id: string): void {
    this.addContactPerson = false;

    const contact: Person = this.scope.session.get(id) as Person;

    const organisationContactRelationship = this.scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.request.Originator as Organisation;
    organisationContactRelationship.Contact = contact;

    this.contacts.push(contact);
    this.request.ContactPerson = contact;
  }

  public partyContactMechanismCancelled() {
    this.addContactMechanism = false;
  }

  public partyContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addContactMechanism = false;

    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
    this.request.FullfillContactMechanism = partyContactMechanism.ContactMechanism;
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
        this.router.navigate(['/orders/request/' + this.request.id]);
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public originatorSelected(party: Party): void {
    if (party) {
      this.update(party);
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }

  private update(party: Party): void {

        const fetches: Fetch[] = [
          new Fetch({
            id: party.id,
            include: [
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: this.m.PostalBoundary.Country }),
                    ],
                    roleType: this.m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: this.m.PartyContactMechanism.ContactMechanism,
              }),
            ],
            name: 'partyContactMechanisms',
            path: new Path({ step: this.m.Party.CurrentPartyContactMechanisms }),
          }),
          new Fetch({
            id: party.id,
            name: 'currentContacts',
            path: new Path({ step: this.m.Party.CurrentContacts }),
          }),
        ];

        this.scope
          .load('Pull', new PullRequest({ fetches }))
          .subscribe((loaded) => {

            if (this.request.Originator !== this.previousOriginator) {
              this.request.ContactPerson = null;
              this.request.FullfillContactMechanism = null;
              this.previousOriginator = this.request.Originator;
            }

            const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
            this.contactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
            this.contacts = loaded.collections.currentContacts as Person[];
          },
          (error: Error) => {
            this.errorService.handle(error);
            this.goBack();
          },
        );
      }
    }
