export * from './catalogue/catalogues-overview.module';
export * from './catalogue/catalogue.module';

export * from './category/categories-overview.module';
export * from './category/category.module';

export * from './eanidentification/edit/eanidentification.module';
export * from './isbnidentification/edit/isbnidentification.module';
export * from './manufactureridentification/edit/manufactureridentification.module';
export * from './partnumber/edit/partnumber.module';
export * from './productnumber/edit/productnumber.module';
export * from './skuidentification/edit/skuidentification.module';
export * from './upcaidentification/edit/upcaidentification.module';
export * from './upceidentification/edit/upceidentification.module';

export * from './baseprice/edit/baseprice.module';

export * from './good/edit/good-edit.module';
export * from './good/list/good-list.module';
export * from './good/overview/good-overview.module';

export * from './part/list/part-list.module';
export * from './part/edit/part-edit.module';
export * from './part/overview/part-overview.module';

export * from './productcharacteristic/productcharacteristics-overview.module';
export * from './productcharacteristic/productcharacteristic.module';

export * from './producttype/producttypes-overview.module';
export * from './producttype/producttype.module';

export * from './serialiseditem/edit/serialiseditem.module';

import { CatalogueModule } from './catalogue/catalogue.module';
import { CataloguesOverviewModule } from './catalogue/catalogues-overview.module';

import { CategoriesOverviewModule } from './category/categories-overview.module';
import { CategoryModule } from './category/category.module';

import { EanIdentificationModule } from './eanidentification/edit/eanidentification.module';
import { IsbnIdentificationModule } from './isbnidentification/edit/isbnidentification.module';
import { ManufacturerIdentificationModule } from './manufactureridentification/edit/manufactureridentification.module';
import { PartNumberModule } from './partnumber/edit/partnumber.module';
import { ProductNumberModule } from './productnumber/edit/productnumber.module';
import { SkuIdentificationModule } from './skuidentification/edit/skuidentification.module';
import { UpcaIdentificationModule } from './upcaidentification/edit/upcaidentification.module';
import { UpceIdentificationModule } from './upceidentification/edit/upceidentification.module';

import { BasepriceModule } from './baseprice/edit/baseprice.module';

import { GoodEditModule } from './good/edit/good-edit.module';
import { GoodListModule } from './good/list/good-list.module';
import { GoodOverviewModule } from './good/overview/good-overview.module';

import { PartEditModule } from './part/edit/part-edit.module';
import { PartListModule } from './part/list/part-list.module';
import { PartOverviewModule } from './part/overview/part-overview.module';

import { ProductCharacteristicModule } from './productcharacteristic/productcharacteristic.module';
import { ProductCharacteristicsOverviewModule } from './productcharacteristic/productcharacteristics-overview.module';

import { ProductTypeModule } from './producttype/producttype.module';
import { ProductTypesOverviewModule } from './producttype/producttypes-overview.module';

import { SerialisedItemModule } from './serialiseditem/edit/serialiseditem.module';

export const Modules = [
  BasepriceModule,
  CataloguesOverviewModule, CatalogueModule,
  CategoriesOverviewModule, CategoryModule,
  EanIdentificationModule, IsbnIdentificationModule, ManufacturerIdentificationModule, PartNumberModule, ProductNumberModule, SkuIdentificationModule, UpcaIdentificationModule, UpceIdentificationModule,
  GoodListModule, GoodOverviewModule,
  PartListModule, PartEditModule, PartOverviewModule,
  ProductCharacteristicsOverviewModule, ProductCharacteristicModule,
  ProductTypesOverviewModule, ProductTypeModule,
  GoodEditModule, SerialisedItemModule
];
