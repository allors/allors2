import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Field, FilterFactory, Invoked, Loaded, Saved, Scope, WorkspaceService } from '../../../../../angular';
import { ContactMechanism, Currency, InternalOrganisation, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, ProductQuote, RequestForQuote } from '../../../../../domain';
import { Contains, Equals, Fetch, Path, PullRequest, Query, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './productquote.component.html',
})
export class ProductQuoteEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title: string;
  public subTitle: string;
  public quote: ProductQuote;
  public request: RequestForQuote;
  public currencies: Currency[];
  public contactMechanisms: ContactMechanism[];
  public contacts: Person[];

  public addContactPerson = false;
  public addContactMechanism = false;

  public scope: Scope;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private previousReceiver: Party;
  private fetcher: Fetcher;

  get showOrganisations(): boolean {
    return !this.quote.Receiver || this.quote.Receiver.objectType.name === 'Organisation';
  }
  get showPeople(): boolean {
    return !this.quote.Receiver || this.quote.Receiver.objectType.name === 'Person';
  }

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    public stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.fetcher = new Fetcher(this.stateService, this.m);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([, , internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const rolesQuery: Query[] = [
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
                  new TreeNode({ roleType: m.ProductQuote.Receiver }),
                  new TreeNode({ roleType: m.ProductQuote.FullfillContactMechanism }),
                  new TreeNode({ roleType: m.ProductQuote.QuoteState }),
                  new TreeNode({ roleType: m.ProductQuote.Request }),
                ],
                name: 'productQuote',
              }),
            ];

            return this.scope.load('Pull', new PullRequest({ fetches }));
          });
      })
      .subscribe((loaded) => {
        this.quote = loaded.objects.productQuote as ProductQuote;
        const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;

        if (!this.quote) {
          this.quote = this.scope.session.create('ProductQuote') as ProductQuote;
          this.quote.Issuer = internalOrganisation;
          this.quote.IssueDate = new Date();
          this.quote.ValidFromDate = new Date();
          this.title = 'Add Quote';
        } else {
          this.title = 'Quote ' + this.quote.QuoteNumber;
        }

        if (this.quote.Receiver) {
          this.title = this.title + ' from: ' + this.quote.Receiver.PartyName;
          this.update(this.quote.Receiver);
        }

        this.previousReceiver = this.quote.Receiver;
      },
      (error: Error) => {
        this.errorService.handle(error);
        this.goBack();
      },
    );
  }

  public personCancelled(): void {
    this.addContactPerson = false;
  }

  public personAdded(id: string): void {
    this.addContactPerson = false;

    const contact: Person = this.scope.session.get(id) as Person;

    const organisationContactRelationship = this.scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.quote.Receiver as Organisation;
    organisationContactRelationship.Contact = contact;

    this.contacts.push(contact);
    this.quote.ContactPerson = contact;
  }

  public partyContactMechanismCancelled() {
    this.addContactMechanism = false;
  }

  public partyContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addContactMechanism = false;

    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.quote.Receiver.AddPartyContactMechanism(partyContactMechanism);
    this.quote.FullfillContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public approve(): void {
    const submitFn: () => void = () => {
      this.scope.invoke(this.quote.Approve)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully approved.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.handle(error);
        });
    };

    if (this.scope.session.hasChanges) {
       this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
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
        }); 
    } else {
      submitFn();
    }
  }

  public reject(): void {
    const rejectFn: () => void = () => {
      this.scope.invoke(this.quote.Reject)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.handle(error);
        });
    };

    if (this.scope.session.hasChanges) {
        this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
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
        }); 
    } else {
      rejectFn();
    }
  }

  public Order(): void {
    const rejectFn: () => void = () => {
      this.scope.invoke(this.quote.Order)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('SalesOrder successfully created.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.handle(error);
        });
    };

    if (this.scope.session.hasChanges) {
        this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
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
        }); 
    } else {
      rejectFn();
    }
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
        this.router.navigate(['/orders/productQuote/' + this.quote.id]);
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public receiverSelected(party: Party): void {
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

  private update(party: Party) {
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

        if (this.quote.Receiver !== this.previousReceiver) {
          this.quote.ContactPerson = null;
          this.quote.FullfillContactMechanism = null;
          this.previousReceiver = this.quote.Receiver;
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
