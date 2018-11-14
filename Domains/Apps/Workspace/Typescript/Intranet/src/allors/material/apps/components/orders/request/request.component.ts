import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Invoked, Saved, Scope, WorkspaceService, x, Allors } from '../../../../../angular';
import { ContactMechanism, Currency, InternalOrganisation, Organisation, OrganisationContactRelationship, Party, PartyContactMechanism, Person, RequestForQuote } from '../../../../../domain';
import { Fetch, PullRequest, TreeNode, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './request.component.html',
  providers: [Allors]
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
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    public stateService: StateService) {

    this.m = this.allors.m;

    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.Currency({ sort: new Sort(m.Currency.Name) })
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded) => {
                scope.session.reset();
                this.currencies = loaded.collections.currencies as Currency[];

                const fetches = [
                  this.fetcher.internalOrganisation,
                  pull.RequestForQuote(
                    {
                      object: id,
                      include: {
                        Currency: x,
                        Originator: x,
                        ContactPerson: x,
                        RequestState: x,
                        FullfillContactMechanism: {
                          PostalAddress_PostalBoundary: {
                            Country: x,
                          }
                        }
                      }
                    }
                  ),
                ];

                return scope.load('Pull', new PullRequest({ pulls: fetches }));
              })
            );
        })
      )

      .subscribe((loaded) => {

        this.request = loaded.objects.requestForQuote as RequestForQuote;
        const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;

        if (!this.request) {
          this.request = scope.session.create('RequestForQuote') as RequestForQuote;
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
      }, this.errorService.handler);
  }

  public submit(): void {
    const { scope } = this.allors;

    const submitFn: () => void = () => {
      scope.invoke(this.request.Submit)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully submitted.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope
              .save()
              .subscribe((saved: Saved) => {
                scope.session.reset();
                submitFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            submitFn();
          }
        });
    } else {
      submitFn();
    }
  }

  public cancel(): void {
    const { scope } = this.allors;

    const cancelFn: () => void = () => {
      scope.invoke(this.request.Cancel)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope
              .save()
              .subscribe((saved: Saved) => {
                scope.session.reset();
                cancelFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public hold(): void {
    const { scope } = this.allors;

    const holdFn: () => void = () => {
      scope.invoke(this.request.Hold)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully held.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope
              .save()
              .subscribe((saved: Saved) => {
                scope.session.reset();
                holdFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            holdFn();
          }
        });
    } else {
      holdFn();
    }
  }

  public reject(): void {
    const { scope } = this.allors;

    const rejectFn: () => void = () => {
      scope.invoke(this.request.Reject)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope
              .save()
              .subscribe((saved: Saved) => {
                scope.session.reset();
                rejectFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            rejectFn();
          }
        });
    } else {
      rejectFn();
    }
  }

  public personCancelled(): void {
    this.addContactPerson = false;
  }

  public personAdded(id: string): void {
    const { scope } = this.allors;

    this.addContactPerson = false;

    const contact: Person = scope.session.get(id) as Person;

    const organisationContactRelationship = scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
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
    const { scope } = this.allors;

    scope
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

    const { m, pull, scope } = this.allors;

    const pulls = [
      pull.Party({
        object: party,
        fetch: {
          CurrentPartyContactMechanisms: {
            include: {
              ContactMechanism: {
                PostalAddress_PostalBoundary: {
                  Country: x
                }
              }
            }
          }
        }
      }),
      pull.Party({
        object: party,
        fetch: {
          CurrentContacts: x
        }
      })
    ];

    scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.request.Originator !== this.previousOriginator) {
          this.request.ContactPerson = null;
          this.request.FullfillContactMechanism = null;
          this.previousOriginator = this.request.Originator;
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.contactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.contacts = loaded.collections.currentContacts as Person[];
      }, this.errorService.handler);
  }
}
