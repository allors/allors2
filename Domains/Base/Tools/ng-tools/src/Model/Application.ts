import { ProjectSymbols } from "ngast";

import { Module } from "./Module";
import { Pipe } from "./Pipe";
import { Provider } from "./Provider";
import { Directive } from "./Directive";

export class Application {

    modules: Module[];
    pipes: Pipe[];
    providers: Provider[];
    directives: Directive[];

    constructor(projectSymbols: ProjectSymbols) {

        this.modules = projectSymbols.getModules().map((v) => new Module(v));
        this.pipes = projectSymbols.getPipes().map((v) => new Pipe(v));
        this.providers = projectSymbols.getProviders().map((v) => new Provider(v));
        this.directives = projectSymbols.getDirectives().map((v) => new Directive(v));
    }
}

