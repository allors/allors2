import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Location } from '@angular/common';

import { Subscription, BehaviorSubject } from 'rxjs';

import { ErrorService, ContextService, MetaService, PanelService, RefreshService } from '../../../../../../angular';
import { Organisation, RequestForQuote, Currency, ContactMechanism, Person, Quote, PartyContactMechanism, OrganisationContactRelationship, Party, CustomerRelationship } from '../../../../../../domain';
import { PullRequest, Sort } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';
import { switchMap, filter } from 'rxjs/operators';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'requestforquote-overview-detail',
  templateUrl: './requestforquote-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class RequestForQuoteOverviewDetailComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  public request: RequestForQuote;
  public quote: Quote;
  public currencies: Currency[];
  public contactMechanisms: ContactMechanism[] = [];
  public contacts: Person[] = [];
  public scope: ContextService;

  public addContactPerson = false;
  public addContactMechanism = false;
  public addOriginator = false;
  private previousOriginator: Party;

  private fetcher: Fetcher;
  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;
  internalOrganisation: Organisation;

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public location: Location,
    private errorService: ErrorService,
    public stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);

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
                  PostalAddress_PostalBoundary: {
                    Country: x,
                  }
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
                    PostalAddress_PostalBoundary: {
                      Country: x,
                    }
                  }
                }
              }
            )
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.request = loaded.objects.RequestForQuote as RequestForQuote;
        this.currencies = loaded.collections.Currencies as Currency[];

        if (this.request.Originator) {
          this.previousOriginator = this.request.Originator;
          this.updateOriginator(this.request.Originator);
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        this.location.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public originatorSelected(party: Party) {
    if (party) {
      this.updateOriginator(party);
    }
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

  public personCancelled(): void {
    this.addContactPerson = false;
  }

  public personAdded(person: Person): void {

    this.addContactPerson = false;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.request.Originator as Organisation;
    organisationContactRelationship.Contact = person;

    this.contacts.push(person);
    this.request.ContactPerson = person;
  }

  public originatorCancelled(): void {
    this.addOriginator = false;
  }

  public originatorAdded(party: Party): void {

    this.addOriginator = false;

    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party as Party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.request.Originator = party;
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
                PostalAddress_PostalBoundary: {
                  Country: x
                }
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
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.request.Originator !== this.previousOriginator) {
          this.request.FullfillContactMechanism = null;
          this.request.ContactPerson = null;
          this.previousOriginator = this.request.Originator;
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.contactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.contacts = loaded.collections.CurrentContacts as Person[];
      }, this.errorService.handler);
  }
}
