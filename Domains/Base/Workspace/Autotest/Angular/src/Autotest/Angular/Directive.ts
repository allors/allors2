import { DirectiveSymbol } from "ngast";
import { PathResolver } from '../Helpers';

import { Template } from './Template';
import { Class } from '../Typescript/Class';

export class Directive {

    name: string;
    path: string;
    isComponent: boolean;
    selector: string;
    template: Template;
    type: Class;

    constructor(public directive: DirectiveSymbol, public pathResolver: PathResolver) {

        const nonResolvedMetadata = this.directive.getNonResolvedMetadata();
        const resolvedMetadata = this.directive.getResolvedMetadata();

        this.name = directive.symbol.name;
        this.path = pathResolver.relative(directive.symbol.filePath);

        this.isComponent = directive.isComponent();
        this.selector = nonResolvedMetadata.selector !== "ng-component" ? nonResolvedMetadata.selector : undefined;

        if (resolvedMetadata && resolvedMetadata.template) {
            this.template = new Template(this, resolvedMetadata, pathResolver);
        }

        const classDeclaration = directive.getNode();
        if (classDeclaration) {
            this.type = new Class(classDeclaration);
        }
    }

    public toJSON(): any {

        const { name, path, isComponent, selector, template, type } = this;

        return {
            kind: 'directive',
            name,
            path,
            isComponent,
            selector,
            template,
            type,
        };
    }
}

