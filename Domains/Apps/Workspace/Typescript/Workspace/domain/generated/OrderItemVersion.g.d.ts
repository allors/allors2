import { SessionObject } from "@allors/framework";
import { PriceableVersion } from './PriceableVersion.g';
import { PurchaseOrder } from './PurchaseOrder.g';
import { QuoteItem } from './QuoteItem.g';
import { OrderTerm } from './OrderTerm.g';
import { OrderItem } from './OrderItem.g';
export interface OrderItemVersion extends SessionObject, PriceableVersion {
    InternalComment: string;
    QuantityOrdered: number;
    Description: string;
    CorrespondingPurchaseOrder: PurchaseOrder;
    TotalOrderAdjustmentCustomerCurrency: number;
    TotalOrderAdjustment: number;
    QuoteItem: QuoteItem;
    AssignedDeliveryDate: Date;
    DeliveryDate: Date;
    OrderTerms: OrderTerm[];
    AddOrderTerm(value: OrderTerm): any;
    RemoveOrderTerm(value: OrderTerm): any;
    ShippingInstruction: string;
    Associations: OrderItem[];
    AddAssociation(value: OrderItem): any;
    RemoveAssociation(value: OrderItem): any;
    Message: string;
}
