import { StaticSymbol } from '@angular/compiler';
import { ModuleSymbol } from 'ngast';

export interface Route {
  path?: string
  pathMatch?: string
  // matcher?: UrlMatcher
  component?: StaticSymbol // Type<any>
  redirectTo?: string
  outlet?: string
  canActivate?: any[]
  canActivateChild?: any[]
  canDeactivate?: any[]
  canLoad?: any[]
  data?: any // Data
  // resolve?: ResolveData
  children?: Route[]
  loadChildren?: string // LoadChildren
  // runGuardsAndResolvers?: RunGuardsAndResolvers

  module?: ModuleSymbol
}



