"use strict";
function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(require("./overview.module"));
__export(require("./dashboard/dashboard.module"));
__export(require("./invoice/invoices-overview.module"));
__export(require("./invoice/invoice-overview.module"));
__export(require("./invoice/invoice.module"));
__export(require("./invoiceitem/invoiceitem.module"));
__export(require("./invoice/invoice-print.module"));
const overview_module_1 = require("./overview.module");
const dashboard_module_1 = require("./dashboard/dashboard.module");
const invoice_overview_module_1 = require("./invoice/invoice-overview.module");
const invoice_print_module_1 = require("./invoice/invoice-print.module");
const invoice_module_1 = require("./invoice/invoice.module");
const invoices_overview_module_1 = require("./invoice/invoices-overview.module");
const invoiceitem_module_1 = require("./invoiceitem/invoiceitem.module");
exports.modules = [
    // Overview
    overview_module_1.OverviewModule,
    dashboard_module_1.DashboardModule,
    invoice_module_1.InvoiceModule, invoice_overview_module_1.InvoiceOverviewModule, invoices_overview_module_1.InvoicesOverviewModule, invoice_print_module_1.InvoicePrintModule, invoiceitem_module_1.InvoiceItemEditModule,
];
