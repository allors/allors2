import { Route, Router } from "@angular/router";
import "rxjs/add/operator/filter";
import "rxjs/add/operator/map";
import { MenuItem } from "./MenuItem";
export declare class MenuService {
    private router;
    menuItems: MenuItem[];
    menuItemByRoute: Map<Route, MenuItem>;
    modules: MenuItem[];
    pagesByModule: Map<MenuItem, MenuItem[]>;
    constructor(router: Router);
    pages(route: Route): MenuItem[];
}
