import { Component, OnInit, Self, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { MetaService, RefreshService, PanelService, ContextService } from '@allors/angular/services/core';
import { Organisation, PartyContactMechanism, Party, Currency, Person, OrganisationContactRelationship, ContactMechanism, CustomerRelationship, RequestForQuote, Quote } from '@allors/domain/generated';
import { SaveService } from '@allors/angular/material/services/core';
import { Meta } from '@allors/meta/generated';
import { Filters, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { Sort } from '@allors/data/system';
import { ISessionObject } from '@allors/domain/system';
import { TestScope, SearchFactory } from '@allors/angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'requestforquote-overview-detail',
  templateUrl: './requestforquote-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class RequestForQuoteOverviewDetailComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  request: RequestForQuote;
  quote: Quote;
  currencies: Currency[];
  contactMechanisms: ContactMechanism[] = [];
  contacts: Person[] = [];
  internalOrganisation: Organisation;

  addContactPerson = false;
  addContactMechanism = false;
  addOriginator = false;
  previousOriginator: Party;

  private subscription: Subscription;

  customersFilter: SearchFactory;

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'detail';
    panel.title = 'Request For Quote Details';
    panel.icon = 'business';
    panel.expandable = true;

    // Collapsed
    const requestForQuotePullName = `${panel.name}_${this.m.RequestForQuote.name}`;
    const productQuotePullName = `${panel.name}_${this.m.ProductQuote.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isCollapsed) {
        const { pull, x } = this.metaService;

        pulls.push(
          pull.RequestForQuote(
            {
              name: requestForQuotePullName,
              object: this.panel.manager.id,
              include: {
                FullfillContactMechanism: {
                  PostalAddress_Country: x
                },
                RequestItems: {
                  Product: x,
                },
                Originator: x,
                ContactPerson: x,
                RequestState: x,
                Currency: x,
                CreatedBy: x,
                LastModifiedBy: x,
              }
            }),
          pull.RequestForQuote(
            {
              name: productQuotePullName,
              object: this.panel.manager.id,
              fetch: {
                QuoteWhereRequest: x
              }
            }
          )
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.request = loaded.objects[requestForQuotePullName] as RequestForQuote;
        this.quote = loaded.objects.Quote as Quote;
      }
    };
  }

  public ngOnInit(): void {

    // Maximized
    this.subscription = this.panel.manager.on$
      .pipe(
        filter(() => {
          return this.panel.isExpanded;
        }),
        switchMap(() => {

          this.request = undefined;

          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Currency({ sort: new Sort(m.Currency.Name) }),
            pull.RequestForQuote(
              {
                object: id,
                include: {
                  Currency: x,
                  Originator: x,
                  ContactPerson: x,
                  RequestState: x,
                  FullfillContactMechanism: {
                    PostalAddress_Country: x
                  }
                }
              }
            )
          ];

          this.customersFilter = Filters.customersFilter(m, this.internalOrganisationId.value);

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.request = loaded.objects.RequestForQuote as RequestForQuote;
        this.currencies = loaded.collections.Currencies as Currency[];

        if (this.request.Originator) {
          this.previousOriginator = this.request.Originator;
          this.update(this.request.Originator);
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        this.refreshService.refresh();
        this.panel.toggle();
      },
        this.saveService.errorHandler
      );
  }

  get originatorIsPerson(): boolean {
    return !this.request.Originator || this.request.Originator.objectType.name === this.m.Person.name;
  }

  public originatorSelected(party: ISessionObject) {
    if (party) {
      this.update(party as Party);
    }
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

  public originatorAdded(party: Party): void {

    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.request.Originator = party;
  }

  private update(party: Party) {

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
