import { ClassDeclaration, Decorator } from "typescript";
import { CompileTemplateMetadata, CompileDirectiveMetadata } from "@angular/compiler";
import { DirectiveSymbol } from "ngast";
import { TemplateAstResult } from "ngast/lib/directive-symbol";
import { Template } from './Template';

export class Directive {

    template: Template;

    resolvedMetadata: CompileTemplateMetadata;
    nonResolvedMetadata: CompileDirectiveMetadata;
    // templateAst: TemplateAstResult;
    // tsNode: ClassDeclaration;
    // decorators: Decorator[];
    // decoratorText: string[];

    constructor(public symbol: DirectiveSymbol) {

        this.resolvedMetadata = symbol.getResolvedMetadata();
        this.nonResolvedMetadata = symbol.getNonResolvedMetadata();
        // this.templateAst = symbol.getTemplateAst();
        // this.tsNode = symbol.getNode();

        // if (this.tsNode) {
        //     this.decorators = this.tsNode.decorators.map((v => v));
        //     this.decoratorText = this.decorators.map((v) => v.getText())
        // }

        if (this.isLocal && this.resolvedMetadata && this.resolvedMetadata.template) {
            this.template = new Template(this);
        }
    }

    get name() { return this.symbol.symbol.name; }

    get isLocal() { return this.symbol.symbol.filePath.indexOf('node_modules') === -1; }

    get isComponent() { return this.symbol.isComponent; }

    get hasSelector() { return this.nonResolvedMetadata.selector !== "ng-component" }

    public toJSON(): any {

        const { name, isLocal, isComponent, hasSelector, template } = this;

        return {
            name,
            isLocal,
            isComponent,
            hasSelector,
            template,
        };
    }
}

