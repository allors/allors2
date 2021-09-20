import { Program } from 'typescript';
import { ProjectSymbols } from './ngast/project-symbols';

import { PathResolver, ResourceResolver } from './Helpers';
import { Module } from './Angular/Module';
import { Pipe } from './Angular/Pipe';
import { Provider } from './Angular/Provider';
import { Directive } from './Angular/Directive';

export class Project {
  modules: Module[];
  pipes: Pipe[];
  providers: Provider[];
  directives: Directive[];
  parseErrors: any[] = [];

  program: Program;

  constructor(public pathResolver: PathResolver, tsConfigPath: string) {
    const projectPath = pathResolver.resolve(tsConfigPath);
    const projectSymbols = new ProjectSymbols(projectPath, new ResourceResolver(), (e) => this.parseErrors.push(e));

    if (this.parseErrors.length === 0) {
      this.program = (projectSymbols as any).program;

      this.modules = projectSymbols
        .getModules()
        .map((v) => {
          try {
            return new Module(v, pathResolver);
          } catch {
            console.debug('Could not resolve module ' + v);
            return undefined;
          }
        })
        .filter((v) => v);

      this.pipes = projectSymbols
        .getPipes()
        .map((v) => {
          try {
            return new Pipe(v, pathResolver);
          } catch {
            console.debug('Could not resolve pipe ' + v);
            return undefined;
          }
        })
        .filter((v) => v);

      this.providers = projectSymbols
        .getProviders()
        .map((v) => {
          try {
            return new Provider(v, pathResolver);
          } catch {
            console.debug('Could not resolve provider ' + v);
            return undefined;
          }
        })
        .filter((v) => v);

      this.directives = projectSymbols
        .getDirectives()
        .map((v) => {
          try {
            return new Directive(v, pathResolver, this.program);
          } catch {
            console.debug('Could not resolve directive ' + v);
            return undefined;
          }
        })
        .filter((v) => v);
    }
  }

  public toJSON(): any {
    const { modules, pipes, providers, directives } = this;

    if (this.parseErrors.length > 0) {
      return {
        parseErrors: this.parseErrors.map((v) => v.toString()),
      };
    }

    return {
      kind: 'project',
      modules,
      pipes,
      providers,
      directives,
    };
  }
}
