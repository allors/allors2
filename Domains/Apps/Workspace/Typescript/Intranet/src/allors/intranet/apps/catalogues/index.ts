export * from "./overview/overview.module";

export * from "./overview/catalogues/cataloguesOverview.module";
export * from "./overview/categories/categoriesOverview.module";
export * from "./overview/dashboard/dashboard.module";
export * from "./overview/goods/goodsOverview.module";
export * from "./overview/productcharacteristics/productcharacteristicsOverview.module";
export * from "./overview/productTypes/productTypesOverview.module";

export * from "./catalogue/edit.module";
export * from "./category/edit.module";
export * from "./good/edit.module";
export * from "./nonSerialisedGood/add.module";
export * from "./productcharacteristic/edit.module";
export * from "./producttype/edit.module";
export * from "./serialisedGood/add.module";

import { OverviewModule } from "./overview/overview.module";

import { CataloguesOverviewModule } from "./overview/catalogues/cataloguesOverview.module";
import { CategoriesOverviewModule } from "./overview/categories/categoriesOverview.module";
import { DashboardModule } from "./overview/dashboard/dashboard.module";
import { GoodsOverviewModule } from "./overview/goods/goodsOverview.module";
import { ProductCharacteristicsOverviewModule } from "./overview/productcharacteristics/productcharacteristicsOverview.module";
import { ProductTypesOverviewModule } from "./overview/productTypes/productTypesOverview.module";

import { CatalogueEditModule } from "./catalogue/edit.module";
import { CategoryEditModule } from "./category/edit.module";
import { GoodEditModule } from "./good/edit.module";
import { NonSerialisedGoodAddModule } from "./nonSerialisedGood/add.module";
import { ProductCharacteristicEditModule } from "./productcharacteristic/edit.module";
import { ProductTypeEditModule } from "./producttype/edit.module";
import { SerialisedGoodAddModule } from "./serialisedGood/add.module";

export const Modules = [
  // Routing
  OverviewModule,
  DashboardModule,

  CataloguesOverviewModule,
  CategoriesOverviewModule,
  GoodsOverviewModule,
  ProductCharacteristicsOverviewModule,
  ProductTypesOverviewModule,

  CatalogueEditModule,
  ProductTypeEditModule,
  CategoryEditModule,
  GoodEditModule,
  NonSerialisedGoodAddModule,
  ProductCharacteristicEditModule,
  SerialisedGoodAddModule,
];
