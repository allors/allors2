export * from './catalogue/catalogues-overview.module';
export * from './catalogue/catalogue.module';

export * from './category/categories-overview.module';
export * from './category/category.module';

export * from './good/list/good-list.module';
export * from './good/newgood-dialog.module';

export * from './productcharacteristic/productcharacteristics-overview.module';
export * from './productcharacteristic/productcharacteristic.module';

export * from './producttype/producttypes-overview.module';
export * from './producttype/producttype.module';

export * from './good/edit/good-edit.module';

import { CatalogueModule } from './catalogue/catalogue.module';
import { CataloguesOverviewModule } from './catalogue/catalogues-overview.module';

import { CategoriesOverviewModule } from './category/categories-overview.module';
import { CategoryModule } from './category/category.module';

import { GoodListModule } from './good/list/good-list.module';
import { NewGoodDialogModule } from './good/newgood-dialog.module';

import { ProductCharacteristicModule } from './productcharacteristic/productcharacteristic.module';
import { ProductCharacteristicsOverviewModule } from './productcharacteristic/productcharacteristics-overview.module';

import { ProductTypeModule } from './producttype/producttype.module';
import { ProductTypesOverviewModule } from './producttype/producttypes-overview.module';

import { GoodEditModule } from './good/edit/good-edit.module';

export const Modules = [
  CataloguesOverviewModule, CatalogueModule,
  CategoriesOverviewModule, CategoryModule,
  GoodListModule, NewGoodDialogModule,
  ProductCharacteristicsOverviewModule, ProductCharacteristicModule,
  ProductTypesOverviewModule, ProductTypeModule,
  GoodEditModule,
];
