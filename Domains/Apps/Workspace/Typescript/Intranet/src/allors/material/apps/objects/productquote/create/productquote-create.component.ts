import { Component, OnDestroy, OnInit, Self, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import {  ContextService, MetaService, RefreshService } from '../../../../../angular';
import { ContactMechanism, Currency, Organisation, OrganisationContactRelationship, Party, PartyContactMechanism, Person, ProductQuote, RequestForQuote, CustomerRelationship } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';

import { CreateData } from '../../../../../material/base/services/object';

@Component({
  templateUrl: './productquote-create.component.html',
  providers: [ContextService]
})
export class ProductQuoteCreateComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  title = 'Add Quote';

  quote: ProductQuote;
  request: RequestForQuote;
  currencies: Currency[];
  contactMechanisms: ContactMechanism[] = [];
  contacts: Person[] = [];

  addContactPerson = false;
  addContactMechanism = false;
  addReceiver = false;
  internalOrganisation: Organisation;

  private subscription: Subscription;
  private previousReceiver: Party;
  private fetcher: Fetcher;

  constructor(
    @Self() public allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: CreateData,
    public dialogRef: MatDialogRef<ProductQuoteCreateComponent>,
    public metaService: MetaService,
    
    public refreshService: RefreshService,
    public stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Currency({ sort: new Sort(m.Currency.Name) })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.quote = loaded.objects.ProductQuote as ProductQuote;
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.currencies = loaded.collections.Currencies as Currency[];

        this.quote = this.allors.context.create('ProductQuote') as ProductQuote;
        this.quote.Issuer = this.internalOrganisation;
        this.quote.IssueDate = new Date();
        this.quote.ValidFromDate = new Date();

      });
  }

  get receiverIsPerson(): boolean {
    return !this.quote.Receiver || this.quote.Receiver.objectType.name === this.m.Person.name;
  }

  public receiverSelected(party: Party): void {
    if (party) {
      this.update(party);
    }
  }

  public receiverAdded(party: Party): void {

    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.quote.Receiver = party;
  }

  public personAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.quote.Receiver as Organisation;
    organisationContactRelationship.Contact = person;

    this.contacts.push(person);
    this.quote.ContactPerson = person;
  }

  public partyContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

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
      .subscribe(() => {
        const data: IObject = {
          id: this.quote.id,
          objectType: this.quote.objectType,
        };

        this.dialogRef.close(data);
      });
  }

  private update(party: Party) {

    const { pull, x } = this.metaService;

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
      });
  }
}
