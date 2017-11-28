import { SessionObject } from "@allors/framework";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
import { ProductCharacteristic } from './ProductCharacteristic.g';
export declare class ProductType extends SessionObject implements UniquelyIdentifiable {
    readonly CanReadProductCharacteristics: boolean;
    readonly CanWriteProductCharacteristics: boolean;
    ProductCharacteristics: ProductCharacteristic[];
    AddProductCharacteristic(value: ProductCharacteristic): void;
    RemoveProductCharacteristic(value: ProductCharacteristic): void;
    readonly CanReadName: boolean;
    readonly CanWriteName: boolean;
    Name: string;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
