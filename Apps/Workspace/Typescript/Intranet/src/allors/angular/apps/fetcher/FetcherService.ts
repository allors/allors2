import { Injectable } from '@angular/core';

import { Pull } from '../../../framework';
import { PullFactory } from '../../../meta';

import { MetaService } from '../../base/framework/MetaService';
import { SingletonId } from '../../base/state/SingletonId';
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
        ActiveSuppliers: x
      }
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
      },
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
