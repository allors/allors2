// Routing
import { CataloguesOverviewComponent } from "./overview/catalogues/cataloguesOverview.component";
import { CategoriesOverviewComponent } from "./overview/categories/categoriesOverview.component";
import { DashboardComponent } from "./overview/dashboard/dashboard.component";
import { GoodsOverviewComponent } from "./overview/goods/goodsOverview.component";
import { ProductCharacteristicsOverviewComponent } from "./overview/productcharacteristics/productcharacteristicsOverview.component";
import { ProductTypesOverviewComponent } from "./overview/productTypes/productTypesOverview.component";

import { CatalogueEditComponent } from "./catalogue/edit.component";
import { CategoryEditComponent } from "./category/edit.component";
import { GoodEditComponent } from "./good/edit.component";
import { NonSerialisedGoodAddComponent } from "./nonSerialisedGood/add.component";
import { ProductCharacteristicEditComponent } from "./productcharacteristic/edit.component";
import { ProductTypeEditComponent } from "./producttype/edit.component";
import { SerialisedGoodAddComponent } from "./serialisedGood/add.component";

export const CATALOGUES: any[] = [
];

export const CATALOGUES_ROUTING: any[] = [
  DashboardComponent,

  CataloguesOverviewComponent,
  CategoriesOverviewComponent,
  GoodsOverviewComponent,
  ProductCharacteristicsOverviewComponent,
  ProductTypesOverviewComponent,

  CatalogueEditComponent,
  CategoryEditComponent,
  GoodEditComponent,
  NonSerialisedGoodAddComponent,
  ProductCharacteristicEditComponent,
  ProductTypeEditComponent,
  SerialisedGoodAddComponent,
];

export {
  // Routing
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
  NonSerialisedGoodAddComponent,
  ProductCharacteristicEditComponent,
  SerialisedGoodAddComponent,
};
