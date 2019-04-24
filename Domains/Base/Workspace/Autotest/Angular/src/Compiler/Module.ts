import { ModuleSymbol } from "ngast";
import { Route } from './Route';

export class Module {
    get name() { return this.symbol.symbol.name; }

    get isLocal() { return this.symbol.symbol.filePath.indexOf('node_modules') === -1; }

    get isMainModule() { return this.symbol.getBootstrapComponents().length > 0; }

    get routes(): Route[] {
        const summary = this.symbol.getModuleSummary();
        if (summary) {
            const routes = summary.providers.filter(v => {
                return v.provider.token.identifier && v.provider.token.identifier.reference.name === 'ROUTES';
            });

            if (routes && routes.length > 0 && routes[0]) {
                const route = routes[0];
                if(route.provider && route.provider.useValue){
                    return route.provider.useValue;
                }
            }
        }

        return [];
    }


    constructor(public symbol: ModuleSymbol) {
    }
}

