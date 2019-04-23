import { ClassDeclaration, Decorator } from "typescript";
import { CompileTemplateMetadata, CompileDirectiveMetadata } from "@angular/compiler";
import { DirectiveSymbol } from "ngast";
import { TemplateAstResult } from "ngast/lib/directive-symbol";

export class Directive {

    resolvedMetadata: CompileTemplateMetadata;
    nonResolvedMetadata: CompileDirectiveMetadata;
    templateAst: TemplateAstResult;
    tsNode: ClassDeclaration;

    decorators: Decorator[];
    decoratorText: string[];

    constructor(public symbol: DirectiveSymbol) {
        this.resolvedMetadata = symbol.getResolvedMetadata();
        this.nonResolvedMetadata = symbol.getNonResolvedMetadata();
        this.templateAst = symbol.getTemplateAst();
        this.tsNode = symbol.getNode();

        if (this.tsNode) {
            this.decorators = this.tsNode.decorators.map((v => v));
            this.decoratorText = this.decorators.map((v) => v.getText())
        }
    }

    get name() { return this.symbol.symbol.name; }

    get isComponent() { return this.symbol.isComponent; }

    get isLocal() { return this.symbol.symbol.filePath.indexOf('node_modules') === -1; }

    get hasSelector() { return this.nonResolvedMetadata.selector !== "ng-component" }
}

