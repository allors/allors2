import { Component, Self, OnInit, OnDestroy, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, NavigationService, PanelService, MediaService, ContextService, RefreshService, Action, ActionTarget, Invoked } from '@allors/angular/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind, SupplierOffering, Part, RatingType, Ordinal, UnitOfMeasure, Currency, Settings, SupplierRelationship, WorkTask, SalesInvoice, FixedAsset, Printable, UnifiedGood, SalesOrder, RepeatingSalesInvoice, Good, WorkEffort, PurchaseOrder, PurchaseInvoice, Shipment, NonUnifiedGood, BasePrice, PriceComponent, ProductIdentificationType } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService } from '@allors/angular/material/core';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { Sort, ContainedIn, Extent, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nonunifiedpart-overview-summary',
  templateUrl: './nonunifiedpart-overview-summary.component.html',
  providers: [PanelService]
})
export class NonUnifiedPartOverviewSummaryComponent {

  m: Meta;

  part: Part;
  serialised: boolean;
  suppliers: string;
  sellingPrice: BasePrice;
  currentPricecomponents: PriceComponent[] = [];
  inactivePricecomponents: PriceComponent[] = [];
  allPricecomponents: PriceComponent[] = [];
  allSupplierOfferings: SupplierOffering[];
  currentSupplierOfferings: SupplierOffering[];
  inactiveSupplierOfferings: SupplierOffering[];
  partnumber: string[];

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService,
    ) {

    this.m = this.metaService.m;

    panel.name = 'summary';

    const partPullName = `${panel.name}_${this.m.Part.name}`;
    const priceComponentPullName = `${panel.name}_${this.m.PriceComponent.name}`;
    const supplierOfferingsPullName = `${panel.name}_${this.m.SupplierOffering.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.PriceComponent({
          name: priceComponentPullName,
          predicate: new Equals({ propertyType: m.PriceComponent.Part, object: id }),
          include: {
            Part: x,
            Currency: x
          },
          sort: new Sort({ roleType: m.PriceComponent.FromDate, descending: true })
        }),
        pull.Part({
          name: partPullName,
          object: id,
          include: {
            ProductIdentifications: {
              ProductIdentificationType: x
            },
            ProductType: x,
            InventoryItemKind: x,
            ManufacturedBy: x,
            SuppliedBy: x,
            SerialisedItems: {
              PrimaryPhoto: x,
              SerialisedItemState: x,
              OwnedBy: x
            },
            Brand: x,
            Model: x
          }
        }),
        pull.Part({
          name: supplierOfferingsPullName,
          object: id,
          fetch: {
            SupplierOfferingsWherePart: {
              include: {
                Currency: x
              }
            }
          }
        }),
        pull.ProductIdentificationType(),
        );
    };

    panel.onPulled = (loaded) => {
      

      this.part = loaded.objects[partPullName] as Part;
      this.serialised = this.part.InventoryItemKind.UniqueId === '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae';

      this.allPricecomponents = loaded.collections[priceComponentPullName] as PriceComponent[];
      this.currentPricecomponents = this.allPricecomponents.filter(v => isBefore(new Date(v.FromDate), new Date()) && (v.ThroughDate === null || isAfter(new Date(v.ThroughDate), new Date())));
      this.inactivePricecomponents = this.allPricecomponents.filter(v => isAfter(new Date(v.FromDate), new Date()) || (v.ThroughDate !== null && isBefore(new Date(v.ThroughDate), new Date())));

      this.allSupplierOfferings = loaded.collections[supplierOfferingsPullName] as SupplierOffering[];
      this.currentSupplierOfferings = this.allSupplierOfferings.filter(v => isBefore(new Date(v.FromDate), new Date()) && (v.ThroughDate === null || isAfter(new Date(v.ThroughDate), new Date())));
      this.inactiveSupplierOfferings = this.allSupplierOfferings.filter(v => isAfter(new Date(v.FromDate), new Date()) || (v.ThroughDate !== null && isBefore(new Date(v.ThroughDate), new Date())));

      const goodIdentificationTypes = loaded.collections.ProductIdentificationTypes as ProductIdentificationType[];
      const partNumberType = goodIdentificationTypes.find((v) => v.UniqueId === '5735191a-cdc4-4563-96ef-dddc7b969ca6');
      this.partnumber = this.part.ProductIdentifications.filter(v => v.ProductIdentificationType === partNumberType).map(w => w.Identification);

      if (this.part.SuppliedBy.length > 0) {
        this.suppliers = this.part.SuppliedBy
          .map(v => v.displayName)
          .reduce((acc: string, cur: string) => acc + ', ' + cur);
      }
    };
  }
}
