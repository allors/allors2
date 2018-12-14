import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, ContextService, MetaService, PanelService, RefreshService } from '../../../../../../angular';
import { Organisation, ProductQuote, Currency, ContactMechanism, Person, Quote, PartyContactMechanism, OrganisationContactRelationship, Good, SalesOrder, InternalOrganisation, Party, RequestForQuote } from '../../../../../../domain';
import { PullRequest, Sort } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap, filter } from 'rxjs/operators';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'productquote-overview-detail',
  templateUrl: './productquote-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class ProductQuoteOverviewDetailComponent implements OnInit, OnDestroy {

  public m: Meta;

  public productQuote: ProductQuote;
  public salesOrder: SalesOrder;
  public request: RequestForQuote;
  public currencies: Currency[];
  public contactMechanisms: ContactMechanism[];
  public contacts: Person[];
  private previousReceiver: Party;

  public addContactPerson = false;
  public addContactMechanism = false;

  private fetcher: Fetcher;
  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public location: Location,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);

    panel.name = 'detail';
    panel.title = 'Request For Quote Details';
    panel.icon = 'business';
    panel.expandable = true;

    // Normal
    const productQuotePullName = `${panel.name}_${this.m.ProductQuote.name}`;
    const salesOrderPullName = `${panel.name}_${this.m.SalesOrder.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isCollapsed) {
        const { pull, x } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.ProductQuote(
            {
              name: productQuotePullName,
              object: this.panel.manager.id,
              include: {
                QuoteItems: {
                  Product: x,
                  QuoteItemState: x,
                },
                Receiver: x,
                ContactPerson: x,
                QuoteState: x,
                CreatedBy: x,
                LastModifiedBy: x,
                Request: x,
                FullfillContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x,
                  }
                }
              }
            }),
          pull.ProductQuote(
            {
              name: salesOrderPullName,
              object: this.panel.manager.id,
              fetch: {
                SalesOrderWhereQuote: x,
              }
            }
          )
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.productQuote = loaded.objects.ProductQuote as ProductQuote;
        this.salesOrder = loaded.objects.SalesOrder as SalesOrder;
      }
    };
  }

  public ngOnInit(): void {

    // Maximized
    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        filter((v) => {
          return this.panel.isExpanded;
        }),
        switchMap(([urlSegments, date, , internalOrganisationId]) => {

          this.productQuote = undefined;

          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Currency({ sort: new Sort(m.Currency.Name) }),
            pull.ProductQuote({
              object: id,
              include: {
                Receiver: x,
                FullfillContactMechanism: x,
                QuoteState: x,
                Request: x,
              }
            })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.productQuote = loaded.objects.ProductQuote as ProductQuote;

      }, this.errorService.handler);
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
        this.panel.toggle();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  get showOrganisations(): boolean {
    return !this.productQuote.Receiver || this.productQuote.Receiver.objectType.name === 'Organisation';
  }

  get showPeople(): boolean {
    return !this.productQuote.Receiver || this.productQuote.Receiver.objectType.name === 'Person';
  }

  public personCancelled(): void {
    this.addContactPerson = false;
  }

  public personAdded(id: string): void {

    this.addContactPerson = false;

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.productQuote.Receiver as Organisation;
    organisationContactRelationship.Contact = contact;

    this.contacts.push(contact);
    this.productQuote.ContactPerson = contact;
  }

  public partyContactMechanismCancelled() {
    this.addContactMechanism = false;
  }

  public partyContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addContactMechanism = false;

    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.productQuote.Receiver.AddPartyContactMechanism(partyContactMechanism);
    this.productQuote.FullfillContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public receiverSelected(party: Party): void {
    if (party) {
      this.update(party);
    }
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

        if (this.productQuote.Receiver !== this.previousReceiver) {
          this.productQuote.ContactPerson = null;
          this.productQuote.FullfillContactMechanism = null;
          this.previousReceiver = this.productQuote.Receiver;
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.contactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.contacts = loaded.collections.currentContacts as Person[];
      }, this.errorService.handler);

  }
}
