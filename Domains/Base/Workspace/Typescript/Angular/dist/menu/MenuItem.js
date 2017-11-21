"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class MenuItem {
    get module() {
        if (this.isModule) {
            return this;
        }
        return this.parent ? this.parent.module : undefined;
    }
    get isModule() {
        return this.type === "module";
    }
    get isPage() {
        return this.type === "page";
    }
    constructor(menuItemByRoute, modules, pagesByModule, route, parent, module) {
        menuItemByRoute.set(route, this);
        this.parent = parent;
        if (route.data) {
            this.type = route.data.type;
            this.title = route.data.title || route.path;
            this.icon = route.data.icon;
        }
        if (!parent) {
            this.link = route.path;
        }
        else {
            this.link = route.path ? parent.link + "/" + route.path : parent.link;
        }
        if (this.isModule) {
            modules.push(this);
            pagesByModule.set(this, []);
        }
        if (this.isPage) {
            pagesByModule.get(module).push(this);
        }
        this.children = route.children ? route.children.map((child) => new MenuItem(menuItemByRoute, modules, pagesByModule, child, this, this.isModule ? this : module)) : [];
    }
}
exports.MenuItem = MenuItem;
