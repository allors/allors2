import { ProjectSymbols } from 'ngast';
import { PathResolver, ResourceResolver } from './Helpers';

import { Module } from "./Angular/Module";
import { Pipe } from "./Angular/Pipe";
import { Provider } from "./Angular/Provider";
import { Directive } from "./Angular/Directive";
import { Program } from './Typescript/Program';

export class Project {

    program: Program;
    modules: Module[];
    pipes: Pipe[];
    providers: Provider[];
    directives: Directive[];

    parseErrors: any[] = [];

    constructor(public pathResolver: PathResolver, tsConfigPath: string) {

        const projectPath = pathResolver.resolve(tsConfigPath);
        const projectSymbols = new ProjectSymbols(projectPath, new ResourceResolver(), e => this.parseErrors.push(e));

        if (this.parseErrors.length === 0) {

            this.program = new Program(projectSymbols, pathResolver);

            this.modules = projectSymbols.getModules().map((v) => new Module(v, pathResolver));
            this.pipes = projectSymbols.getPipes().map((v) => new Pipe(v, pathResolver));
            this.providers = projectSymbols.getProviders().map((v) => new Provider(v, pathResolver));
            this.directives = projectSymbols.getDirectives().map((v) => new Directive(v, pathResolver, this.program));
        }
    }

    public toJSON(): any {

        const { program, modules, pipes, providers, directives } = this;

        if (this.parseErrors.length > 0) {
            return {
                parseErrors: this.parseErrors.map((v) => v.toString())
            }
        }

        return {
            kind: 'project',
            program,
            // modules,
            // pipes,
            // providers,
            // directives,
        };
    }
}

