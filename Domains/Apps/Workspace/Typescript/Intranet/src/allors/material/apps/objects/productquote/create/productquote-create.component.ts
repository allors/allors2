import { Component, OnDestroy, OnInit, Self, Inject, Optional } from '@angular/core';
import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { ContactMechanism, Currency, InternalOrganisation, Organisation, OrganisationContactRelationship, Party, PartyContactMechanism, Person, ProductQuote, RequestForQuote } from '../../../../../domain';
import { PullRequest, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

import { CreateData, ObjectService, ObjectData } from '../../../../../material/base/services/object';

@Component({
  templateUrl: './productquote-create.component.html',
  providers: [ContextService]
})
export class ProductQuoteCreateComponent implements OnInit, OnDestroy {

  public m: Meta;

  public title = 'Add Quote';

  public quote: ProductQuote;
  public request: RequestForQuote;
  public currencies: Currency[];
  public contactMechanisms: ContactMechanism[];
  public contacts: Person[];

  public addContactPerson = false;
  public addContactMechanism = false;

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
    @Self() public allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: CreateData,
    public dialogRef: MatDialogRef<ProductQuoteCreateComponent>,
    public metaService: MetaService,
    private errorService: ErrorService,
    public refreshService: RefreshService,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    public stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, internalOrganisationId]) => {

          const pulls = [
            pull.Currency(
              {
                sort: new Sort(m.Currency.Name),
              }
            )
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded) => {
                this.allors.context.reset();
                this.currencies = loaded.collections.Currencies as Currency[];

                const pulls2 = [
                  this.fetcher.internalOrganisation,
                ];

                return this.allors.context.load('Pull', new PullRequest({ pulls: pulls2 }));
              })
            );
        })
      )
      .subscribe((loaded) => {
        this.quote = loaded.objects.ProductQuote as ProductQuote;
        const internalOrganisation = loaded.objects.InternalOrganisation as Organisation;

        this.quote = this.allors.context.create('ProductQuote') as ProductQuote;
        this.quote.Issuer = internalOrganisation;
        this.quote.IssueDate = new Date();
        this.quote.ValidFromDate = new Date();

      }, this.errorService.handler);
  }

  public personCancelled(): void {
    this.addContactPerson = false;
  }

  public personAdded(id: string): void {

    this.addContactPerson = false;

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
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

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        const data: ObjectData = {
          id: this.quote.id,
          objectType: this.quote.objectType,
        };

        this.dialogRef.close(data);
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

  public goBack(): void {
    window.history.back();
  }

  private update(party: Party) {

    const { m, pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
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
        }
      ),
      pull.Party({
        object: party,
        fetch: {
          CurrentContacts: x
        }
      })
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.quote.Receiver !== this.previousReceiver) {
          this.quote.ContactPerson = null;
          this.quote.FullfillContactMechanism = null;
          this.previousReceiver = this.quote.Receiver;
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.contactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.contacts = loaded.collections.CurrentContacts as Person[];
      }, this.errorService.handler);

  }
}
