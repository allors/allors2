// Routing
import { OverviewComponent } from "./overview/overview.component";

import { CataloguesOverviewComponent } from "./overview/catalogues/cataloguesOverview.component";
import { CategoriesOverviewComponent } from "./overview/categories/categoriesOverview.component";
import { DashboardComponent } from "./overview/dashboard/dashboard.component";
import { GoodsOverviewComponent } from "./overview/goods/goodsOverview.component";
import { ProductCharacteristicsOverviewComponent } from "./overview/productcharacteristics/productcharacteristicsOverview.component";
import { ProductTypesOverviewComponent } from "./overview/productTypes/productTypesOverview.component";

import { CatalogueEditComponent } from "./catalogue/edit.component";
import { CategoryEditComponent } from "./category/edit.component";
import { GoodEditComponent } from "./good/edit.component";
import { ProductCharacteristicEditComponent } from "./productcharacteristic/edit.component";
import { ProductTypeEditComponent } from "./producttype/edit.component";

export const CATALOGUES: any[] = [
];

export const CATALOGUES_ROUTING: any[] = [
  OverviewComponent,
  DashboardComponent,

  CataloguesOverviewComponent,
  CategoriesOverviewComponent,
  GoodsOverviewComponent,
  ProductCharacteristicsOverviewComponent,
  ProductTypesOverviewComponent,

  CatalogueEditComponent,
  CategoryEditComponent,
  GoodEditComponent,
  ProductCharacteristicEditComponent,
  ProductTypeEditComponent,
];

export {
  // Routing
  OverviewComponent,
  DashboardComponent,

  CataloguesOverviewComponent,
  CategoriesOverviewComponent,
  GoodsOverviewComponent,
  ProductCharacteristicsOverviewComponent,
  ProductTypesOverviewComponent,

  CatalogueEditComponent,
  ProductTypeEditComponent,
  CategoryEditComponent,
  GoodEditComponent,
  ProductCharacteristicEditComponent,
};
