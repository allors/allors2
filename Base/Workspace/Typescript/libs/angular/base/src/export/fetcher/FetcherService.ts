import { Injectable } from '@angular/core';
import { PullFactory } from '@allors/meta/generated';
import { SingletonId, MetaService } from '@allors/angular/services/core';
import { Pull, ContainedIn, Extent, Equals, Sort, And } from '@allors/data/system';

import { InternalOrganisationId } from '../state/InternalOrganisationId';

const x = {};

@Injectable({
  providedIn: 'root',
})
export class FetcherService {
  pull: PullFactory;

  constructor(
    private singletonId: SingletonId,
    private internalOrganisationId: InternalOrganisationId,
    private meta: MetaService) {

    this.pull = this.meta.pull;
  }

  public get internalOrganisation(): Pull {

    return this.pull.Organisation({
      name: 'InternalOrganisation',
      object: this.internalOrganisationId.value,
      include: {
        DefaultPaymentMethod: x,
        DefaultShipmentMethod: x,
        DefaultCollectionMethod: x,
        PaymentMethods: x,
        ActiveCollectionMethods: x,
        ActiveCustomers: x,
        ActiveEmployees: x,
        ActiveSuppliers: x,
        FacilitiesWhereOwner: x,
        PreferredCurrency: x,
        Locale: x,
        OrderAddress: x,
        BillingAddress: x,
        ShippingAddress: x,
        GeneralCorrespondence: x,
        Country: {
          DerivedVatRegimes: {
            VatRates: x,
          }
        }
      }
    });
  }

  public get warehouses(): Pull {
    const { m } = this.meta;

    return this.pull.Facility({
      predicate: new ContainedIn({
        propertyType: m.Facility.FacilityType,
        extent: new Extent({
          objectType: m.FacilityType,
          predicate: new Equals({
            propertyType: m.FacilityType.UniqueId,
            value: 'd4a70252-58d0-425b-8f54-7f55ae01a7b3',
          })
        })
      }),
      include: {
        Owner: x,
      },
      sort: new Sort(m.Facility.Name),
    });
  }

  public get ownWarehouses(): Pull {
    const { m } = this.meta;

    return this.pull.Facility({
        predicate: new And([
          new Equals({
            propertyType: m.Facility.Owner,
            object: this.internalOrganisationId.value,
          }),
          new ContainedIn({
            propertyType: m.Facility.FacilityType,
            extent: new Extent({
              objectType: m.FacilityType,
              predicate: new Equals({
                propertyType: m.FacilityType.UniqueId,
                value: 'd4a70252-58d0-425b-8f54-7f55ae01a7b3',
              }),
            }),
        }),
      ]),
      sort: new Sort(m.Facility.Name),
    });
  }

  public get categories(): Pull {

    return this.pull.Organisation({
      object: this.internalOrganisationId.value,
      fetch: { ProductCategoriesWhereInternalOrganisation: x },
    });
  }

  public get locales(): Pull {

    return this.pull.Singleton({
      object: this.singletonId.value,
      fetch: {
        AdditionalLocales: {
          include: {
            Language: x,
            Country: x
          }
        }
      }
    });
  }

  public get Settings(): Pull {

    return this.pull.Singleton({
      object: this.singletonId.value,
      fetch: {
        Settings: {
          include: {
            PreferredCurrency: x,
            DefaultFacility: x
          }
        }
      },
    });
  }
}
