import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { PullRequest } from '@allors/protocol/system';
import { ObjectData, SaveService } from '@allors/angular/material/services/core';
import {
  Organisation,
  CustomerRelationship,
  Currency,
  Person,
  OrganisationContactRelationship,
  Party,
  PartyContactMechanism,
  ContactMechanism,
  RequestForQuote,
} from '@allors/domain/generated';
import { Sort } from '@allors/data/system';
import { FetcherService, InternalOrganisationId, Filters } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Meta } from '@allors/meta/generated';
import { TestScope, SearchFactory } from '@allors/angular/core';

@Component({
  templateUrl: './requestforquote-create.component.html',
  providers: [ContextService]
})
export class RequestForQuoteCreateComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title = 'Add Request for Quote';

  request: RequestForQuote;
  currencies: Currency[];
  contactMechanisms: ContactMechanism[] = [];
  contacts: Person[] = [];
  internalOrganisation: Organisation;
  scope: ContextService;

  addContactPerson = false;
  addContactMechanism = false;
  addOriginator = false;

  private previousOriginator: Party;
  private subscription: Subscription;

  customersFilter: SearchFactory;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<RequestForQuoteCreateComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull } = this.metaService;

    this.subscription = combineLatest([this.refreshService.refresh$, this.internalOrganisationId.observable$])
      .pipe(
        switchMap(([, internalOrganisationId]) => {

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Currency({ sort: new Sort(m.Currency.Name) })
          ];

          this.customersFilter = Filters.customersFilter(m, internalOrganisationId);

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.currencies = loaded.collections.Currencies as Currency[];

        this.request = this.allors.context.create('RequestForQuote') as RequestForQuote;
        this.request.Recipient = this.internalOrganisation;
        this.request.RequestDate = new Date().toISOString();

      });
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
          id: this.request.id,
          objectType: this.request.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  get originatorIsPerson(): boolean {
    return !this.request.Originator || this.request.Originator.objectType.name === this.m.Person.name;
  }

  public originatorSelected(party: ISessionObject) {
    if (party) {
      this.updateOriginator(party as Party);
    }
  }

  public originatorAdded(party: Party): void {

    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.request.Originator = party;
  }

  public partyContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
    this.request.FullfillContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public personAdded(person: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.request.Originator as Organisation;
    organisationContactRelationship.Contact = person;

    this.contacts.push(person);
    this.request.ContactPerson = person;
  }

  private updateOriginator(party: Party) {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party.id,
        fetch: {
          CurrentPartyContactMechanisms: {
            include: {
              ContactMechanism: {
                PostalAddress_Country: x
              }
            }
          }
        },
      }),
      pull.Party({
        object: party.id,
        fetch: {
          CurrentContacts: x
        }
      })
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.request.Originator !== this.previousOriginator) {
          this.request.FullfillContactMechanism = null;
          this.request.ContactPerson = null;
          this.previousOriginator = this.request.Originator;
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.contactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.contacts = loaded.collections.CurrentContacts as Person[];
      });
  }
}
