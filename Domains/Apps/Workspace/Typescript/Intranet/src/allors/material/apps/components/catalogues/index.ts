export * from './catalogue/catalogues-overview.module';
export * from './catalogue/catalogue.module';

export * from './category/categories-overview.module';
export * from './category/category.module';

export * from './goodidentification/edit/goodidentification.module';

export * from './good/edit/good-edit.module';
export * from './good/list/good-list.module';
export * from './good/overview/good-overview.module';

export * from './part/list/part-list.module';
export * from './part/edit/part-edit.module';

export * from './productcharacteristic/productcharacteristics-overview.module';
export * from './productcharacteristic/productcharacteristic.module';

export * from './producttype/producttypes-overview.module';
export * from './producttype/producttype.module';

import { CatalogueModule } from './catalogue/catalogue.module';
import { CataloguesOverviewModule } from './catalogue/catalogues-overview.module';

import { CategoriesOverviewModule } from './category/categories-overview.module';
import { CategoryModule } from './category/category.module';

import { GoodidentIdentificationModule } from './goodidentification/edit/goodidentification.module';

import { GoodEditModule } from './good/edit/good-edit.module';
import { GoodListModule } from './good/list/good-list.module';
import { GoodOverviewModule } from './good/overview/good-overview.module';

import { PartEditModule } from './part/edit/part-edit.module';
import { PartListModule } from './part/list/part-list.module';

import { ProductCharacteristicModule } from './productcharacteristic/productcharacteristic.module';
import { ProductCharacteristicsOverviewModule } from './productcharacteristic/productcharacteristics-overview.module';

import { ProductTypeModule } from './producttype/producttype.module';
import { ProductTypesOverviewModule } from './producttype/producttypes-overview.module';

export const Modules = [
  CataloguesOverviewModule, CatalogueModule,
  CategoriesOverviewModule, CategoryModule,
  GoodidentIdentificationModule, GoodListModule, GoodOverviewModule,
  PartListModule, PartEditModule,
  ProductCharacteristicsOverviewModule, ProductCharacteristicModule,
  ProductTypesOverviewModule, ProductTypeModule,
  GoodEditModule,
];
