import { Pull } from '../../../framework';
import { StateService } from '../services/StateService';
import { PullFactory } from '../../../meta';
import { x, Allors } from '../../../angular';

export class Fetcher {

  constructor(private stateService: StateService, private pull: PullFactory) {
  }

  public get internalOrganisation(): Pull {

    return this.pull.InternalOrganisation({
      name: 'InternalOrganisation',
      object: this.stateService.internalOrganisationId,
      include: {
        DefaultPaymentMethod: x,
        DefaultShipmentMethod: x,
        DefaultFacility: x,
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
      object: this.stateService.internalOrganisationId,
      fetch: { ProductCategoriesWhereInternalOrganisation: x },
    });
  }

  public get locales(): Pull {

    return this.pull.Singleton({
      object: this.stateService.singletonId,
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
}
