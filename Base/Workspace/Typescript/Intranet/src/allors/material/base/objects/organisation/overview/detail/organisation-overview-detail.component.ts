import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, PanelService, FetcherService, TestScope, SingletonId } from '../../../../../../angular';
import { CustomOrganisationClassification, IndustryClassification, InternalOrganisation, Locale, Organisation, LegalForm } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { SaveService } from '../../../../../../material';
import { switchMap, filter } from 'rxjs/operators';
import { VatRegime, Currency } from '../../../../../../domain/generated';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'organisation-overview-detail',
  templateUrl: './organisation-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class OrganisationOverviewDetailComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  organisation: Organisation;
  locales: Locale[];
  classifications: CustomOrganisationClassification[];
  industries: IndustryClassification[];
  internalOrganisation: InternalOrganisation;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  legalForms: LegalForm[];
  vatRegimes: VatRegime[];
  currencies: Currency[];

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public saveService: SaveService,
    public location: Location,
    private route: ActivatedRoute,
    private singletonId: SingletonId,
    private fetcher: FetcherService
  ) {
    super();

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    panel.name = 'detail';
    panel.title = 'Organisation Details';
    panel.icon = 'business';
    panel.expandable = true;

    // Collapsed
    const pullName = `${this.panel.name}_${this.m.Organisation.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isCollapsed) {
        const { pull } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.Organisation({
            name: pullName,
            object: id,
          })
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.organisation = loaded.objects[pullName] as Organisation;
      }
    };
  }

  public ngOnInit(): void {

    // Expanded
    this.subscription = this.panel.manager.on$
      .pipe(
        filter(() => {
          return this.panel.isExpanded;
        }),
        switchMap(() => {
          this.organisation = undefined;

          const { m, x, pull } = this.metaService;
          const id = this.panel.manager.id;

          const pulls = [
            this.fetcher.internalOrganisation,
            this.fetcher.locales,
            pull.Singleton({
              object: this.singletonId.value,
              fetch: {
                Locales: {
                  include: {
                    Language: x,
                    Country: x
                  }
                }
              }
            }),
            pull.Organisation({ object: id }),
            pull.Currency({
              predicate: new Equals({ propertyType: m.Currency.IsActive, value: true }),
              sort: new Sort(m.Currency.Name),
            }),
            pull.CustomOrganisationClassification({
              sort: new Sort(m.CustomOrganisationClassification.Name)
            }),
            pull.IndustryClassification({
              sort: new Sort(m.IndustryClassification.Name)
            }),
            pull.LegalForm({
              sort: new Sort(m.LegalForm.Description)
            }),
            pull.VatRegime({
              sort: new Sort(m.VatRegime.Name)
            })
          ];

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.organisation = loaded.objects.Organisation as Organisation;
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.currencies = loaded.collections.Currencies as Currency[];
        this.locales = loaded.collections.AdditionalLocales as Locale[] || [];
        this.locales.push(loaded.objects.DefaultLocale as Locale);
        this.classifications = loaded.collections.CustomOrganisationClassifications as CustomOrganisationClassification[];
        this.industries = loaded.collections.IndustryClassifications as IndustryClassification[];
        this.legalForms = loaded.collections.LegalForms as LegalForm[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
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
        window.history.back();
      },
        this.saveService.errorHandler
      );
  }
}
