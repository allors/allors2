import { SessionObject, Method } from "@allors/framework";
import { Product } from './Product.g';
export interface Service extends SessionObject, Product {
    CanExecuteDelete: boolean;
    Delete: Method;
}
