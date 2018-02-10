import { Fetch, Path, TreeNode } from "../../../framework";
import { MetaDomain } from "../../../meta";
import { StateService } from "../services/StateService";

export class Fetcher {

  constructor(private stateService: StateService, private m: MetaDomain) {
  }

  public get internalOrganisation(): Fetch {
    return new Fetch({
      id: this.stateService.internalOrganisationId,
      include: [
        new TreeNode({ roleType: this.m.InternalOrganisation.DefaultFacility}),
        new TreeNode({ roleType: this.m.InternalOrganisation.DefaultCollectionMethod}),
        new TreeNode({ roleType: this.m.InternalOrganisation.DefaultPaymentMethod}),
        new TreeNode({ roleType: this.m.InternalOrganisation.DefaultShipmentMethod}),
        new TreeNode({ roleType: this.m.InternalOrganisation.PaymentMethods}),
        new TreeNode({ roleType: this.m.InternalOrganisation.ActiveCollectionMethods}),
      ],
      name: "internalOrganisation",
    });
  }

  public get categories(): Fetch {
    return new Fetch({
      id: this.stateService.internalOrganisationId,
      name: "categories",
      path: new Path({ step: this.m.InternalOrganisation.ProductCategoriesWhereInternalOrganisation }),
    });
  }

  public get locales(): Fetch {
    return new Fetch({
      id: this.stateService.singletonId,
      include: [
        new TreeNode({ roleType: this.m.Locale.Language}),
        new TreeNode({ roleType: this.m.Locale.Country}),
      ],
      path: new Path({ step: this.m.Singleton.AdditionalLocales }),
      name: "locales",
    });
  }

}
