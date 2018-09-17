import { Pull } from '../../../framework';
import { StateService } from '../services/StateService';
import { DataService, x } from '../../../angular';

export class Fetcher {

  constructor(private stateService: StateService, private dataService: DataService) {
  }

  public get internalOrganisation(): Pull {
    const { pull } = this.dataService;

    return pull.InternalOrganisation({
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
    const { pull } = this.dataService;

    return pull.Organisation({
      object: this.stateService.internalOrganisationId,
      fetch: { ProductCategoriesWhereInternalOrganisation: x },
    });
  }

  public get locales(): Pull {

    const { pull, tree } = this.dataService;

    return pull.Singleton({
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
