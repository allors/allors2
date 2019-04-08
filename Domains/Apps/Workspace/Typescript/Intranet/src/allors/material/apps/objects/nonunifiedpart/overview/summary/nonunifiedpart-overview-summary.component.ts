import * as moment from 'moment';

import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService } from '../../../../../../angular';
import { BasePrice, PriceComponent, SupplierOffering, Part } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { StateService } from '../../../../../../material';
import { Equals, Sort } from 'src/allors/framework';

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

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService,
    private stateService: StateService) {

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
      );
    };

    panel.onPulled = (loaded) => {
      const now = moment.utc();

      this.part = loaded.objects[partPullName] as Part;
      this.serialised = this.part.InventoryItemKind.UniqueId === '2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE'.toLowerCase();

      this.allPricecomponents = loaded.collections[priceComponentPullName] as PriceComponent[];
      this.currentPricecomponents = this.allPricecomponents.filter(v => moment(v.FromDate).isBefore(now) && (v.ThroughDate === null || moment(v.ThroughDate).isAfter(now)));
      this.inactivePricecomponents = this.allPricecomponents.filter(v => moment(v.FromDate).isAfter(now) || (v.ThroughDate !== null && moment(v.ThroughDate).isBefore(now)));

      this.allSupplierOfferings = loaded.collections[supplierOfferingsPullName] as SupplierOffering[];
      this.currentSupplierOfferings = this.allSupplierOfferings.filter(v => moment(v.FromDate).isBefore(now) && (v.ThroughDate === null || moment(v.ThroughDate).isAfter(now)));
      this.inactiveSupplierOfferings = this.allSupplierOfferings.filter(v => moment(v.FromDate).isAfter(now) || (v.ThroughDate !== null && moment(v.ThroughDate).isBefore(now)));

      if (this.part.SuppliedBy.length > 0) {
        this.suppliers = this.part.SuppliedBy
          .map(v => v.displayName)
          .reduce((acc: string, cur: string) => acc + ', ' + cur);
      }
    };
  }
}
