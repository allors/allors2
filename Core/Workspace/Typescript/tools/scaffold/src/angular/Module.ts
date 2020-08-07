import { StaticSymbol } from '@angular/compiler';
import { ModuleSymbol } from '../ngast/module-symbol';

import { PathResolver } from '../Helpers';
import { Route } from './Route';
import { Reference } from './Reference';

export class Module {
  name: string;
  path: string;

  bootstrapComponents: Reference[];
  declaredDirectives: Reference[];
  exportedDirectives: Reference[];
  exportedPipes: Reference[];
  declaredPipes: Reference[];
  importedModules: Reference[];
  exportedModules: Reference[];

  entryComponents: Reference[];

  routes: Route[];

  constructor(public module: ModuleSymbol, pathResolver: PathResolver) {
    this.name = module.symbol.name;
    this.path = pathResolver.relative(module.symbol.filePath);

    this.bootstrapComponents = Reference.fromSymbols(
      module.getBootstrapComponents(),
      pathResolver
    );
    this.declaredDirectives = Reference.fromSymbols(
      module.getDeclaredDirectives(),
      pathResolver
    );
    this.exportedDirectives = Reference.fromSymbols(
      module.getExportedDirectives(),
      pathResolver
    );
    this.exportedPipes = Reference.fromSymbols(
      module.getExportedPipes(),
      pathResolver
    );
    this.declaredPipes = Reference.fromSymbols(
      module.getDeclaredPipes(),
      pathResolver
    );
    this.importedModules = Reference.fromSymbols(
      module.getImportedModules(),
      pathResolver
    );
    this.exportedModules = Reference.fromSymbols(
      module.getExportedModules(),
      pathResolver
    );

    module.getProviders();
    const summary = this.module.getModuleSummary();
    if (summary) {
      if (summary.entryComponents && summary.entryComponents.length) {
        this.entryComponents = Reference.fromSymbols(
          summary.entryComponents
            .map((v) => {
              if (v.componentType instanceof StaticSymbol) {
                return v.componentType;
              }

              if (v.componentFactory instanceof StaticSymbol) {
                return v.componentFactory;
              }

              // TODO: find a member that is a StaticSymbol
              return undefined;
            })
            .filter((v) => v),
          pathResolver
        );
      }

      const routes = summary.providers.filter((v) => {
        return (
          v.provider.token.identifier &&
          v.provider.token.identifier.reference.name === 'ROUTES'
        );
      });

      if (routes && routes.length > 0 && routes[0]) {
        const route = routes[0];
        if (route.provider && route.provider.useValue) {
          this.routes = route.provider.useValue.map(
            (v: any) => new Route(v, pathResolver)
          );
        }
      }
    }
  }

  public toJSON(): any {
    const {
      name,
      path,
      bootstrapComponents,
      declaredDirectives,
      exportedDirectives,
      exportedPipes,
      declaredPipes,
      importedModules,
      exportedModules,
      entryComponents,
      routes,
    } = this;

    return {
      kind: 'module',
      name,
      path,
      bootstrapComponents,
      declaredDirectives,
      exportedDirectives,
      exportedPipes,
      declaredPipes,
      importedModules,
      exportedModules,
      entryComponents,
      routes,
    };
  }
}
