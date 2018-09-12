import { Pull } from '../../../framework';
import { StateService } from '../services/StateService';
import { DataService, x } from '../../../angular';

export class Fetcher {

  constructor(private state: StateService, private data: DataService) {
  }

  public get internalOrganisation(): Pull {
    const { pull } = this.data;

    return pull.InternalOrganisation({
      object: this.state.internalOrganisationId,
      include: {
        Party_DefaultPaymentMethod: x,
        Party_DefaultShipmentMethod: x,
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
    const { pull } = this.data;

    return pull.Organisation({
      object: this.state.internalOrganisationId,
      path: { ProductCategoriesWhereInternalOrganisation: x },
    });
  }

  public get locales(): Pull {

    const { pull, tree } = this.data;

    return pull.Singleton({
      object: this.state.singletonId,
      path: { AdditionalLocales: x },
      include: tree.Locale({
        Language: x,
        Country: x
      })
    });
  }
}
