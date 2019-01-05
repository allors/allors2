import { Component, OnDestroy, OnInit, Self, SkipSelf } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { ErrorService, ContextService, NavigationService, PanelService, RefreshService, MetaService } from '../../../../../../angular';
import { InternalOrganisation, Locale, Organisation, Good, Facility, ProductCategory, ProductType, Brand, Model, VendorProduct, Ownership, VatRate, Part, GoodIdentificationType, ProductNumber } from '../../../../../../domain';
import { PullRequest, Sort } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'good-overview-detail',
  templateUrl: './good-overview-detail.component.html',
  providers: [PanelService, ContextService]
})
export class GoodOverviewDetailComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  good: Good;

  facility: Facility;
  locales: Locale[];
  categories: ProductCategory[];
  productTypes: ProductType[];
  manufacturers: Organisation[];
  brands: Brand[];
  selectedBrand: Brand;
  models: Model[];
  selectedModel: Model;
  vendorProduct: VendorProduct;
  vatRates: VatRate[];
  ownerships: Ownership[];
  organisations: Organisation[];
  addBrand = false;
  addModel = false;
  parts: Part[];
  goodIdentificationTypes: GoodIdentificationType[];
  productNumber: ProductNumber;

  private subscription: Subscription;
  private fetcher: Fetcher;

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    private metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public location: Location,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);

    panel.name = 'detail';
    panel.title = 'Good Details';
    panel.icon = 'person';
    panel.expandable = true;

    // Minimized
    const pullName = `${this.panel.name}_${this.m.Good.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.Good({
          name: pullName,
          object: id,
          include: {
            GoodIdentifications: {
              GoodIdentificationType: x
            },
            Part: {
              Brand: x,
              Model: x
            }
          }
        }),
      );
    };

    panel.onPulled = (loaded) => {
      this.good = loaded.objects[pullName] as Good;
    };
  }

  public ngOnInit(): void {

    // Maximized
    this.subscription = combineLatest(
      this.route.url,
      this.route.queryParams,
      this.refreshService.refresh$,
      this.stateService.internalOrganisationId$,
    )
      .pipe(
        filter((v) => {
          return this.panel.isExpanded;
        }),
        switchMap(([, , , internalOrganisationId]) => {

          this.good = undefined;

          const { m, pull, x } = this.metaService;
          const fetcher = new Fetcher(this.stateService, this.metaService.pull);
          const id = this.panel.manager.id;

          const pulls = [
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
            pull.VatRate(),
            pull.GoodIdentificationType(),
            pull.ProductCategory({ sort: new Sort(m.ProductCategory.Name) }),
            pull.Part({
              include: {
                Brand: x,
                Model: x
              },
              sort: new Sort(m.Part.Name),
            }),
            pull.Good({
              object: id,
              include: {
                Part: {
                  Brand: x,
                  Model: x
                },
                PrimaryPhoto: x,
                GoodIdentifications: x,
                Photos: x,
                ElectronicDocuments: x,
                LocalisedNames: {
                  Locale: x,
                },
                LocalisedDescriptions: {
                  Locale: x,
                },
                LocalisedComments: {
                  Locale: x,
                },
              },
            }),
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.facility = internalOrganisation.DefaultFacility;

        this.good = loaded.objects.Good as Good;
        this.categories = loaded.collections.ProductCategories as ProductCategory[];
        this.parts = loaded.collections.Parts as Part[];
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.goodIdentificationTypes = loaded.collections.GoodIdentificationTypes as GoodIdentificationType[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];

        const goodNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === 'b640630d-a556-4526-a2e5-60a84ab0db3f');

        this.productNumber = this.good.GoodIdentifications.find(v => v.GoodIdentificationType === goodNumberType);

      }, this.errorService.handler);

  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
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
}
