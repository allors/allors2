import { LayoutComponent } from './layout.component';
// Routing
import { CatalogueDashboardComponent } from './dashboard/catalogue-dashboard.component';
import { CatalogueFormComponent } from './catalogues/catalogue/catalogue.component';
import { CataloguesComponent } from './catalogues/catalogues.component';
import { CategoryFormComponent } from './categories/category/category.component';
import { CategoriesComponent } from './categories/categories.component';
import { GoodFormComponent } from './goods/good/good.component';
import { GoodsComponent } from './goods/goods.component';
import { ProductTypeFormComponent } from './producttypes/producttype/producttype.component';
import { ProductTypesComponent } from './producttypes/producttypes.component';
import { ProductCharacteristicFormComponent } from './productCharacteristics/productcharacteristic/productcharacteristic.component';
import { ProductCharacteristicsComponent } from './productcharacteristics/productcharacteristics.component';

export const CATALOGUES: any[] = [
  LayoutComponent,
];

export const CATALOGUES_ROUTING: any[] = [
  CatalogueDashboardComponent,
  CatalogueFormComponent,
  CataloguesComponent,
  CategoryFormComponent,
  CategoriesComponent,
  GoodFormComponent,
  GoodsComponent,
  ProductCharacteristicFormComponent,
  ProductCharacteristicsComponent,
  ProductTypeFormComponent,
  ProductTypesComponent,
];

export {
  // Routing
  CatalogueDashboardComponent,
  CatalogueFormComponent,
  CataloguesComponent,
  CategoryFormComponent,
  CategoriesComponent,
  GoodFormComponent,
  GoodsComponent,
  ProductCharacteristicFormComponent,
  ProductCharacteristicsComponent,
  ProductTypeFormComponent,
  ProductTypesComponent,
};
