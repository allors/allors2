"use strict";
function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(require("./overview.module"));
__export(require("./dashboard/dashboard.module"));
__export(require("./catalogue/catalogues-overview.module"));
__export(require("./catalogue/catalogue.module"));
__export(require("./category/categories-overview.module"));
__export(require("./category/category.module"));
__export(require("./good/goods-overview.module"));
__export(require("./good/newgood-dialog.module"));
__export(require("./productcharacteristic/productcharacteristics-overview.module"));
__export(require("./productcharacteristic/productcharacteristic.module"));
__export(require("./producttype/producttypes-overview.module"));
__export(require("./producttype/producttype.module"));
__export(require("./nonSerialisedGood/nonserialisedgoodmodule"));
__export(require("./serialisedGood/serialisedgood.module"));
const dashboard_module_1 = require("./dashboard/dashboard.module");
const overview_module_1 = require("./overview.module");
const catalogue_module_1 = require("./catalogue/catalogue.module");
const catalogues_overview_module_1 = require("./catalogue/catalogues-overview.module");
const categories_overview_module_1 = require("./category/categories-overview.module");
const category_module_1 = require("./category/category.module");
const goods_overview_module_1 = require("./good/goods-overview.module");
const newgood_dialog_module_1 = require("./good/newgood-dialog.module");
const productcharacteristic_module_1 = require("./productcharacteristic/productcharacteristic.module");
const productcharacteristics_overview_module_1 = require("./productcharacteristic/productcharacteristics-overview.module");
const producttype_module_1 = require("./producttype/producttype.module");
const producttypes_overview_module_1 = require("./producttype/producttypes-overview.module");
const nonserialisedgoodmodule_1 = require("./nonSerialisedGood/nonserialisedgoodmodule");
const serialisedgood_module_1 = require("./serialisedGood/serialisedgood.module");
exports.Modules = [
    // Routing
    overview_module_1.OverviewModule,
    dashboard_module_1.DashboardModule,
    catalogues_overview_module_1.CataloguesOverviewModule, catalogue_module_1.CatalogueModule,
    categories_overview_module_1.CategoriesOverviewModule, category_module_1.CategoryModule,
    goods_overview_module_1.GoodsOverviewModule, newgood_dialog_module_1.NewGoodDialogModule,
    productcharacteristics_overview_module_1.ProductCharacteristicsOverviewModule, productcharacteristic_module_1.ProductCharacteristicModule,
    producttypes_overview_module_1.ProductTypesOverviewModule, producttype_module_1.ProductTypeModule,
    nonserialisedgoodmodule_1.NonSerialisedGoodModule,
    serialisedgood_module_1.SerialisedGoodModule,
];
