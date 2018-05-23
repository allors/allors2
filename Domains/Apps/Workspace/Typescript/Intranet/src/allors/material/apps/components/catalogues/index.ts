export * from './catalogue/catalogues-overview.module';
export * from './catalogue/catalogue.module';

export * from './category/categories-overview.module';
export * from './category/category.module';

export * from './good/goods-overview.module';
export * from './good/newgood-dialog.module';

export * from './productcharacteristic/productcharacteristics-overview.module';
export * from './productcharacteristic/productcharacteristic.module';

export * from './producttype/producttypes-overview.module';
export * from './producttype/producttype.module';

export * from './nonSerialisedGood/nonserialisedgoodmodule';
export * from './serialisedGood/serialisedgood.module';

import { CatalogueModule } from './catalogue/catalogue.module';
import { CataloguesOverviewModule } from './catalogue/catalogues-overview.module';

import { CategoriesOverviewModule } from './category/categories-overview.module';
import { CategoryModule } from './category/category.module';

import { GoodsOverviewModule } from './good/goods-overview.module';
import { NewGoodDialogModule } from './good/newgood-dialog.module';

import { ProductCharacteristicModule } from './productcharacteristic/productcharacteristic.module';
import { ProductCharacteristicsOverviewModule } from './productcharacteristic/productcharacteristics-overview.module';

import { ProductTypeModule } from './producttype/producttype.module';
import { ProductTypesOverviewModule } from './producttype/producttypes-overview.module';

import { NonSerialisedGoodModule } from './nonSerialisedGood/nonserialisedgoodmodule';
import { SerialisedGoodModule } from './serialisedGood/serialisedgood.module';

export const Modules = [
  CataloguesOverviewModule, CatalogueModule,
  CategoriesOverviewModule, CategoryModule,
  GoodsOverviewModule, NewGoodDialogModule,
  ProductCharacteristicsOverviewModule, ProductCharacteristicModule,
  ProductTypesOverviewModule, ProductTypeModule,
  NonSerialisedGoodModule,
  SerialisedGoodModule,
];
