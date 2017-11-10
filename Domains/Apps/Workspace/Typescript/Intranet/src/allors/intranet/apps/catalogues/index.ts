export * from "./overview.module";
export * from "./dashboard/dashboard.module";

export * from "./catalogue/catalogues-overview.module";
export * from "./catalogue/catalogue.module";

export * from "./category/categories-overview.module";
export * from "./category/category.module";

export * from "./good/goods-overview.module";
export * from "./good/good.module";
export * from "./good/newgood-dialog.module";

export * from "./productcharacteristic/productcharacteristics-overview.module";
export * from "./productcharacteristic/productcharacteristic.module";

export * from "./producttype/producttypes-overview.module";
export * from "./producttype/producttype.module";

export * from "./nonSerialisedGood/nonserialisedgood-add.module";
export * from "./serialisedGood/serialisedgood-add.module";

import { DashboardModule } from "./dashboard/dashboard.module";
import { OverviewModule } from "./overview.module";

import { CatalogueModule } from "./catalogue/catalogue.module";
import { CataloguesOverviewModule } from "./catalogue/catalogues-overview.module";

import { CategoriesOverviewModule } from "./category/categories-overview.module";
import { CategoryModule } from "./category/category.module";

import { GoodModule } from "./good/good.module";
import { GoodsOverviewModule } from "./good/goods-overview.module";
import { NewGoodDialogModule } from "./good/newgood-dialog.module";

import { ProductCharacteristicModule } from "./productcharacteristic/productcharacteristic.module";
import { ProductCharacteristicsOverviewModule } from "./productcharacteristic/productcharacteristics-overview.module";

import { ProductTypeModule } from "./producttype/producttype.module";
import { ProductTypesOverviewModule } from "./producttype/producttypes-overview.module";

import { NonSerialisedGoodAddModule } from "./nonSerialisedGood/nonserialisedgood-add.module";
import { SerialisedGoodAddModule } from "./serialisedGood/serialisedgood-add.module";

export const Modules = [
  // Routing
  OverviewModule,
  DashboardModule,

  CataloguesOverviewModule, CatalogueModule,
  CategoriesOverviewModule, CategoryModule,
  GoodsOverviewModule, GoodModule, NewGoodDialogModule,
  ProductCharacteristicsOverviewModule, ProductCharacteristicModule,
  ProductTypesOverviewModule, ProductTypeModule,
  NonSerialisedGoodAddModule,
  SerialisedGoodAddModule,
];
