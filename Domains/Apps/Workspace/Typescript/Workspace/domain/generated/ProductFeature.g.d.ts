import { SessionObject } from "@allors/framework";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
import { EstimatedProductCost } from './EstimatedProductCost.g';
import { PriceComponent } from './PriceComponent.g';
import { VatRate } from './VatRate.g';
export interface ProductFeature extends SessionObject, UniquelyIdentifiable {
    EstimatedProductCosts: EstimatedProductCost[];
    AddEstimatedProductCost(value: EstimatedProductCost): any;
    RemoveEstimatedProductCost(value: EstimatedProductCost): any;
    BasePrices: PriceComponent[];
    Description: string;
    DependentFeatures: ProductFeature[];
    AddDependentFeature(value: ProductFeature): any;
    RemoveDependentFeature(value: ProductFeature): any;
    IncompatibleFeatures: ProductFeature[];
    AddIncompatibleFeature(value: ProductFeature): any;
    RemoveIncompatibleFeature(value: ProductFeature): any;
    VatRate: VatRate;
}
