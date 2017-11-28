import { Route } from "@angular/router";
export declare class MenuItem {
    route: Route;
    parent: MenuItem;
    children: MenuItem[];
    type: "module" | "page";
    title: string;
    icon?: string;
    link: string;
    readonly module: MenuItem;
    readonly isModule: boolean;
    readonly isPage: boolean;
    constructor(menuItemByRoute: Map<Route, MenuItem>, modules: MenuItem[], pagesByModule: Map<MenuItem, MenuItem[]>, route: Route, parent?: MenuItem, module?: MenuItem);
}
