import { ProjectSymbols } from "ngast";
import { ResourceResolver } from "./ResourceResolver";
import { Application } from "./Model/Application";

export class Program {

    readonly application: Application;

    constructor(public projectPath: string) {

        let parseError: any;
        const projectSymbols = new ProjectSymbols(projectPath, new ResourceResolver(), e => (parseError = e));

        if(!parseError){
            this.application = new Application(projectSymbols);
        }
    }
}

