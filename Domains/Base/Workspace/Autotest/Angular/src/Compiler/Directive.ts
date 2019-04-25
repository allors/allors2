import { CompileTemplateMetadata, CompileDirectiveMetadata } from "@angular/compiler";
import { DirectiveSymbol } from "ngast";
import { Template } from './Template';
import { Class } from './Typescript/Class';

export class Directive {

    resolvedMetadata: CompileTemplateMetadata;
    nonResolvedMetadata: CompileDirectiveMetadata;

    name: string;
    isLocal: boolean;
    isComponent: boolean;
    hasSelector: boolean;
    template: Template;
    cls: Class;

    constructor(public symbol: DirectiveSymbol) {

        this.resolvedMetadata = symbol.getResolvedMetadata();
        this.nonResolvedMetadata = symbol.getNonResolvedMetadata();

        this.name = symbol.symbol.name;
        this.isLocal = symbol.symbol.filePath.indexOf('node_modules') === -1;
        this.isComponent = symbol.isComponent(); 
        this.hasSelector = this.nonResolvedMetadata.selector !== "ng-component";

        if (this.isLocal) {
            if (this.resolvedMetadata && this.resolvedMetadata.template) {
                this.template = new Template(this);
            }

            const classDeclaration = symbol.getNode();
            if (classDeclaration) {
                this.cls = new Class(classDeclaration);
            }
        }
    }

    public toJSON(): any {

        const { name, isLocal, isComponent, hasSelector, template, cls } = this;

        return {
            name,
            isLocal,
            isComponent,
            hasSelector,
            template,
            class: cls,
        };
    }
}

