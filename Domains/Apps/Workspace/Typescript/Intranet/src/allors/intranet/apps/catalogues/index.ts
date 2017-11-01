export * from "./overview.module";
export * from "./dashboard/dashboard.module";

export * from "./catalogues/catalogues-overview.module";
export * from "./catalogues/catalogue/catalogue.module";

export * from "./categories/categories-overview.module";
export * from "./categories/category/category.module";

export * from "./goods/goods-overview.module";
export * from "./goods/good/good.module";

export * from "./productcharacteristics/productcharacteristics-overview.module";
export * from "./productcharacteristics/productcharacteristic/productcharacteristic.module";

export * from "./productTypes/producttypes-overview.module";
export * from "./productTypes/producttype/producttype.module";

export * from "./nonSerialisedGood/nonserialisedgood.module";
export * from "./serialisedGood/serialisedgood.module";

import { DashboardModule } from "./dashboard/dashboard.module";
import { OverviewModule } from "./overview.module";

import { CatalogueModule } from "./catalogues/catalogue/catalogue.module";
import { CataloguesOverviewModule } from "./catalogues/catalogues-overview.module";

import { CategoriesOverviewModule } from "./categories/categories-overview.module";
import { CategoryModule } from "./categories/category/category.module";

import { GoodModule } from "./goods/good/good.module";
import { GoodsOverviewModule } from "./goods/goods-overview.module";

import { ProductCharacteristicModule } from "./productcharacteristics/productcharacteristic/productcharacteristic.module";
import { ProductCharacteristicsOverviewModule } from "./productcharacteristics/productcharacteristics-overview.module";

import { ProductTypeModule } from "./productTypes/producttype/producttype.module";
import { ProductTypesOverviewModule } from "./productTypes/producttypes-overview.module";

import { NonSerialisedGoodModule } from "./nonSerialisedGood/nonserialisedgood.module";
import { SerialisedGoodModule } from "./serialisedGood/serialisedgood.module";

export const Modules = [
  // Routing
  OverviewModule,
  DashboardModule,

  CataloguesOverviewModule, CatalogueModule,
  CategoriesOverviewModule, CategoryModule,
  GoodsOverviewModule, GoodModule,
  ProductCharacteristicsOverviewModule, ProductCharacteristicModule,
  ProductTypesOverviewModule, ProductTypeModule,
  NonSerialisedGoodModule,
  SerialisedGoodModule,
];
