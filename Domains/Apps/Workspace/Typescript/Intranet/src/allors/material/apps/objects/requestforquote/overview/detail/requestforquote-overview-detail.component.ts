import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, ContextService, MetaService, PanelService, RefreshService } from '../../../../../../angular';
import { CustomerRelationship, CustomOrganisationClassification, IndustryClassification, InternalOrganisation, Locale, Organisation, OrganisationRole, SupplierRelationship, RequestForQuote, Currency, ContactMechanism, Person, Party, Quote, PartyContactMechanism, OrganisationContactRelationship } from '../../../../../../domain';
import { And, Equals, Exists, Not, PullRequest, Sort } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap, filter } from 'rxjs/operators';
import { load } from '@angular/core/src/render3';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'requestforquote-overview-detail',
  templateUrl: './requestforquote-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class RequestForQuoteOverviewDetailComponent implements OnInit, OnDestroy {

  public m: Meta;

  public requestForQuote: RequestForQuote;
  public quote: Quote;
  public currencies: Currency[];
  public contactMechanisms: ContactMechanism[];
  public contacts: Person[];
  public scope: ContextService;

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
    const requestForQuotePullName = `${panel.name}_${this.m.RequestForQuote.name}`;
    const productQuotePullName = `${panel.name}_${this.m.ProductQuote.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isCollapsed) {
        const { pull, x } = this.metaService;
        const id = this.panel.manager.id;

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
        this.requestForQuote = loaded.objects[requestForQuotePullName] as RequestForQuote;
        this.quote = loaded.objects.quote as Quote;
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

          this.requestForQuote = undefined;

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

        this.requestForQuote = loaded.objects.RequestForQuote as RequestForQuote;
        this.currencies = loaded.collections.Currencies as Currency[];

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

  public partyContactMechanismCancelled() {
    this.addContactMechanism = false;
  }

  public partyContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addContactMechanism = false;

    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.requestForQuote.Originator.AddPartyContactMechanism(partyContactMechanism);
    this.requestForQuote.FullfillContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public personCancelled(): void {
    this.addContactPerson = false;
  }

  public personAdded(id: string): void {

    this.addContactPerson = false;

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.requestForQuote.Originator as Organisation;
    organisationContactRelationship.Contact = contact;

    this.contacts.push(contact);
    this.requestForQuote.ContactPerson = contact;
  }
}
