import * as moment from 'moment/moment';

import { Component, OnDestroy, OnInit, Self, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, InternalOrganisationId, FetcherService, TestScope } from '../../../../../angular';
import { SaveService, FiltersService } from '../../../../../material';
import { ContactMechanism, Currency, Organisation, OrganisationContactRelationship, Party, PartyContactMechanism, Person, ProductQuote, RequestForQuote, CustomerRelationship, VatRegime, IrpfRegime } from '../../../../../domain';
import { PullRequest, Sort, IObject, ISessionObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';

import { ObjectData } from '../../../../../material/core/services/object';

@Component({
  templateUrl: './productquote-create.component.html',
  providers: [ContextService]
})
export class ProductQuoteCreateComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title = 'Add Quote';

  quote: ProductQuote;
  request: RequestForQuote;
  currencies: Currency[];
  contactMechanisms: ContactMechanism[] = [];
  contacts: Person[] = [];
  vatRegimes: VatRegime[];
  irpfRegimes: IrpfRegime[];

  addContactPerson = false;
  addContactMechanism = false;
  addReceiver = false;
  internalOrganisation: Organisation;

  private subscription: Subscription;
  private previousReceiver: Party;

  constructor(
    @Self() public allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<ProductQuoteCreateComponent>,
    public metaService: MetaService,
    public filtersService: FiltersService,
    private saveService: SaveService,
    public refreshService: RefreshService,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId) {

    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Currency({ sort: new Sort(m.Currency.Name) }),
            pull.VatRegime({ sort: new Sort(m.VatRegime.Name) }),
            pull.IrpfRegime({ sort: new Sort(m.IrpfRegime.Name) })
          ];

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.quote = loaded.objects.ProductQuote as ProductQuote;
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.currencies = loaded.collections.Currencies as Currency[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.irpfRegimes = loaded.collections.IrpfRegimes as IrpfRegime[];

        this.quote = this.allors.context.create('ProductQuote') as ProductQuote;
        this.quote.Issuer = this.internalOrganisation;
        this.quote.Currency = this.internalOrganisation.PreferredCurrency;
        this.quote.IssueDate = moment.utc().toISOString();
        this.quote.ValidFromDate = moment.utc().toISOString();

      });
  }

  get receiverIsPerson(): boolean {
    return !this.quote.Receiver || this.quote.Receiver.objectType.name === this.m.Person.name;
  }

  public receiverSelected(party: ISessionObject): void {
    if (party) {
      this.update(party as Party);
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
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
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
                  PostalAddress_Country: x
                }
              }
            }
          }
        }
      ),
      pull.Party({
        object: party,
        fetch: {
          CurrentContacts: x,
        }
      })
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.quote.Receiver !== this.previousReceiver) {
          this.quote.ContactPerson = null;
          this.quote.FullfillContactMechanism = null;
          this.previousReceiver = this.quote.Receiver;
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.contactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.contacts = loaded.collections.CurrentContacts as Person[];

        this.quote.Currency = this.quote.Receiver.PreferredCurrency;
      });
  }
}
