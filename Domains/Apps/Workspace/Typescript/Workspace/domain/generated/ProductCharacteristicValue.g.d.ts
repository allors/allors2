import { SessionObject, Method } from "@allors/framework";
import { Deletable } from './Deletable.g';
import { Localised } from './Localised.g';
import { ProductCharacteristic } from './ProductCharacteristic.g';
import { Locale } from './Locale.g';
export declare class ProductCharacteristicValue extends SessionObject implements Deletable, Localised {
    readonly CanReadProductCharacteristic: boolean;
    readonly CanWriteProductCharacteristic: boolean;
    ProductCharacteristic: ProductCharacteristic;
    readonly CanReadValue: boolean;
    readonly CanWriteValue: boolean;
    Value: string;
    readonly CanReadLocale: boolean;
    readonly CanWriteLocale: boolean;
    Locale: Locale;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
