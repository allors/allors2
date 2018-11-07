import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, Scope, x, Allors, NavigationService, NavigationActivatedRoute } from '../../../../../../angular';
import { Facility, Good, InternalOrganisation, Locale, Organisation, Ownership, ProductType, SupplierOffering, VendorProduct, SerialisedItem } from '../../../../../../domain';
import { Equals, PullRequest, Sort } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/StateService';
import { Fetcher } from '../../../Fetcher';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './serialiseditem.component.html',
  providers: [Allors]
})
export class EditSerialisedItemComponent implements OnInit, OnDestroy {

  m: MetaDomain;
  scope: Scope;
  item: SerialisedItem;

  add: boolean;
  edit: boolean;

  title: string;
  subTitle: string;
  facility: Facility;
  locales: Locale[];
  productTypes: ProductType[];
  suppliers: Organisation[];
  activeSuppliers: Organisation[];
  selectedSuppliers: Organisation[];
  supplierOfferings: SupplierOffering[];
  ownerships: Ownership[];
  organisations: Organisation[];

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;

  constructor(
    @Self() private allors: Allors,
    public navigationService: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService,
  ) {

    this.m = this.allors.m;
    this.scope = this.allors.scope;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, , internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.param();

          const add = !id;

          const pulls = [
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
            pull.SerialisedItem({
              object: id,
              include: {
                PrimaryPhoto: x,
                Photos: x,
                Ownership: x,
                SerialisedItemCharacteristics: {
                  SerialisedItemCharacteristicType: {
                    UnitOfMeasure: x,
                  },
                  LocalisedValues: {
                    Locale: x,
                  }
                }
              }
            }),
            pull.Product(
              {
                object: id,
                fetch: {
                  // TODO:
                  // SupplierOfferingsWhereProduct: x
                }
              }
            ),
            pull.Ownership({
              sort: new Sort(m.Ownership.Name),
            }),
            pull.SerialisedInventoryItemState({
              predicate: new Equals({ propertyType: m.SerialisedInventoryItemState.IsActive, value: true }),
              sort: new Sort(m.SerialisedInventoryItemState.Name),
            }),
            pull.ProductType({ sort: new Sort(m.ProductType.Name) }
            ),
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {
        scope.session.reset();

        this.item = loaded.objects.SerialisedItem as SerialisedItem;
        this.productTypes = loaded.collections.productTypes as ProductType[];
        this.ownerships = loaded.collections.ownerships as Ownership[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;
        this.facility = internalOrganisation.DefaultFacility;
        this.activeSuppliers = internalOrganisation.ActiveSuppliers as Organisation[];
        this.activeSuppliers = this.activeSuppliers.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));

        if (this.item === undefined) {
          this.add = !(this.edit = false);

          this.item = scope.session.create('SerialisedItem') as SerialisedItem;


        } else {
          this.edit = !(this.add = false);
        }

      },
        (error: any) => {
          this.errorService.handle(error);
          this.navigationService.back();
        },
      );
  }
  // const pulls2 = [
  //   pull.Organisation({
  //     predicate: new Equals({ propertyType: m.Organisation.IsManufacturer, value: true }),
  //     sort: new Sort(m.Organisation.PartyName),
  //   })
  // ];
  // this.manufacturers = loaded.collections.manufacturers as Organisation[];


  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public save(): void {
    const { scope } = this.allors;

    this.onSave();

    scope
      .save()
      .subscribe(() => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public update(): void {
    const { scope } = this.allors;

    this.onSave();

    scope
      .save()
      .subscribe(() => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goBack(): void {
    window.history.back();
  }

  private onSave() {

    if (this.suppliers !== undefined) {
      const suppliersToDelete = this.suppliers.filter(v => v);

      if (this.selectedSuppliers !== undefined) {
        this.selectedSuppliers.forEach((supplier: Organisation) => {
          const index = suppliersToDelete.indexOf(supplier);
          if (index > -1) {
            suppliersToDelete.splice(index, 1);
          }

          const now = new Date();
          const supplierOffering = this.supplierOfferings.find((v) =>
            v.Supplier === supplier &&
            v.FromDate <= now &&
            (v.ThroughDate === null || v.ThroughDate >= now));

          if (supplierOffering === undefined) {
            this.supplierOfferings.push(this.newSupplierOffering(supplier));
          } else {
            supplierOffering.ThroughDate = null;
          }
        });
      }

      if (suppliersToDelete !== undefined) {
        suppliersToDelete.forEach((supplier: Organisation) => {
          const now = new Date();
          const supplierOffering = this.supplierOfferings.find((v) =>
            v.Supplier === supplier &&
            v.FromDate <= now &&
            (v.ThroughDate === null || v.ThroughDate >= now));

          if (supplierOffering !== undefined) {
            supplierOffering.ThroughDate = new Date();
          }
        });
      }
    }
  }

  private newSupplierOffering(supplier: Organisation): SupplierOffering {
    const { scope } = this.allors;

    const supplierOffering = scope.session.create('SupplierOffering') as SupplierOffering;
    supplierOffering.Supplier = supplier;
    // TODO:
    // supplierOffering.Product = good;
    return supplierOffering;
  }
}
