"use strict";
function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(require("./overview.module"));
__export(require("./order/orders-overview.module"));
__export(require("./productquote/productquote-overview.module"));
__export(require("./productquote/productquotes-overview.module"));
__export(require("./productquote/productquote-print.module"));
__export(require("./request/request-overview.module"));
__export(require("./request/requests-overview.module"));
__export(require("./salesorder/salesorder-overview.module"));
__export(require("./salesorder/salesorders-overview.module"));
__export(require("./salesorder/salesorder-print.module"));
__export(require("./productquote/productquote.module"));
__export(require("./quoteitem/quoteitem.module"));
__export(require("./request/request.module"));
__export(require("./requestitem/requestitem.module"));
__export(require("./salesorder/salesorder.module"));
__export(require("./salesorderitem/salesorderitem.module"));
const overview_module_1 = require("./overview.module");
const orders_overview_module_1 = require("./order/orders-overview.module");
const productquote_overview_module_1 = require("./productquote/productquote-overview.module");
const productquote_print_module_1 = require("./productquote/productquote-print.module");
const productquotes_overview_module_1 = require("./productquote/productquotes-overview.module");
const request_overview_module_1 = require("./request/request-overview.module");
const requests_overview_module_1 = require("./request/requests-overview.module");
const salesorder_overview_module_1 = require("./salesorder/salesorder-overview.module");
const salesorder_print_module_1 = require("./salesorder/salesorder-print.module");
const salesorders_overview_module_1 = require("./salesorder/salesorders-overview.module");
const productquote_module_1 = require("./productquote/productquote.module");
const quoteitem_module_1 = require("./quoteitem/quoteitem.module");
const request_module_1 = require("./request/request.module");
const requestitem_module_1 = require("./requestitem/requestitem.module");
const salesorder_module_1 = require("./salesorder/salesorder.module");
const salesorderitem_module_1 = require("./salesorderitem/salesorderitem.module");
exports.Modules = [
    // Overview
    overview_module_1.OverviewModule,
    orders_overview_module_1.OrdersOverviewModule,
    productquote_overview_module_1.ProductQuoteOverviewModule,
    productquotes_overview_module_1.ProductQuotesOverviewModule,
    productquote_print_module_1.ProductQuotePrintModule,
    requests_overview_module_1.RequestsOverviewModule,
    request_overview_module_1.RequestOverviewModule,
    salesorder_overview_module_1.SalesOrderOverviewModule,
    salesorders_overview_module_1.SalesOrdersOverviewModule,
    salesorder_print_module_1.SalesOrderPrintModule,
    productquote_module_1.ProductQuoteEditModule,
    request_module_1.RequestEditModule,
    quoteitem_module_1.QuoteItemEditModule,
    requestitem_module_1.RequestItemEditModule,
    salesorder_module_1.SalesOrderEditModule,
    salesorderitem_module_1.SalesOrderItemEditModule,
];
