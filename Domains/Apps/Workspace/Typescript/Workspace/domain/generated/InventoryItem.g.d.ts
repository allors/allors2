import { SessionObject } from "@allors/framework";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
import { ProductCharacteristicValue } from './ProductCharacteristicValue.g';
import { InventoryItemVariance } from './InventoryItemVariance.g';
import { Part } from './Part.g';
import { Lot } from './Lot.g';
import { UnitOfMeasure } from './UnitOfMeasure.g';
import { Good } from './Good.g';
import { ProductType } from './ProductType.g';
import { Facility } from './Facility.g';
export interface InventoryItem extends SessionObject, UniquelyIdentifiable {
    ProductCharacteristicValues: ProductCharacteristicValue[];
    AddProductCharacteristicValue(value: ProductCharacteristicValue): any;
    RemoveProductCharacteristicValue(value: ProductCharacteristicValue): any;
    InventoryItemVariances: InventoryItemVariance[];
    AddInventoryItemVariance(value: InventoryItemVariance): any;
    RemoveInventoryItemVariance(value: InventoryItemVariance): any;
    Part: Part;
    Name: string;
    Lot: Lot;
    Sku: string;
    UnitOfMeasure: UnitOfMeasure;
    Good: Good;
    ProductType: ProductType;
    Facility: Facility;
}
