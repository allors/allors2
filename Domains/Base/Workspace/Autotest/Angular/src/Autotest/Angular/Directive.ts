import { DirectiveSymbol } from "ngast";
import { PathResolver } from '../Helpers';

import { Program } from '../Typescript/Program';
import { Type } from '../Typescript/Type';
import { Template } from './Template';

export class Directive {

    name: string;
    path: string;
    isComponent: boolean;
    selector: string;
    exportAs: string;
    template: Template;
    type: Type;

    constructor(public directive: DirectiveSymbol, public pathResolver: PathResolver, public program: Program) {

        const nonResolvedMetadata = this.directive.getNonResolvedMetadata();
        const resolvedMetadata = this.directive.getResolvedMetadata();

        this.name = directive.symbol.name;
        this.path = pathResolver.relative(directive.symbol.filePath);

        this.isComponent = directive.isComponent();
        this.selector = nonResolvedMetadata.selector !== "ng-component" ? nonResolvedMetadata.selector : undefined;
        this.exportAs = nonResolvedMetadata.exportAs;

        if (resolvedMetadata && resolvedMetadata.template) {
            this.template = new Template(this, resolvedMetadata, pathResolver);
        }

        const classDeclaration = directive.getNode();
        if (classDeclaration) {
            this.type = this.program.lookupOrMap(classDeclaration)
        }
    }

    public toJSON(): any {

        const { name, path, isComponent, selector, exportAs, template, type } = this;

        return {
            kind: 'directive',
            name,
            path,
            isComponent,
            selector,
            exportAs,
            template,
            type: type ? type.name : null,
        };
    }
}

