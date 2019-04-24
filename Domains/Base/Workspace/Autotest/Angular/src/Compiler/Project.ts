import { ProjectSymbols, ResourceResolver as NgastResourceResolver, ErrorReporter } from 'ngast';
import { readFile, readFileSync } from 'fs';

import { Module } from "./Module";
import { Pipe } from "./Pipe";
import { Provider } from "./Provider";
import { Directive } from "./Directive";

class ResourceResolver implements NgastResourceResolver {

    public get(url: string): Promise<string> {
        return new Promise((resolve, reject) => {
            readFile(url, 'utf-8', (err, content) => {
                if (err) {
                    reject(err);
                } else {
                    resolve(content);
                }
            });
        });
    }

    getSync(url: string) {
        return readFileSync(url).toString();
    }
};

export class Project {

    modules: Module[];
    pipes: Pipe[];
    providers: Provider[];
    directives: Directive[];

    parseErrors: any[] = [];

    mainModule: Module;

    constructor(projectPath: string) {

        const projectSymbols = new ProjectSymbols(projectPath, new ResourceResolver(), e => this.parseErrors.push(e));

        if (this.parseErrors.length === 0) {
            this.modules = projectSymbols.getModules().map((v) => new Module(v));
            this.pipes = projectSymbols.getPipes().map((v) => new Pipe(v));
            this.providers = projectSymbols.getProviders().map((v) => new Provider(v));
            this.directives = projectSymbols.getDirectives().map((v) => new Directive(v));

            this.mainModule = this.modules.find(m => m.isMainModule)
        }
    }
}

