import { CompileTemplateMetadata } from '@angular/compiler';
import { PathResolver } from './Helpers';
import { Directive } from './Directive';
import { Node, nodeFactory } from './Html/Node';

export class Template {

    url: string;
    html: Node[];

    constructor(public directive: Directive, resolvedMetadata: CompileTemplateMetadata, pathResolver: PathResolver) {

        this.url = pathResolver.relative(resolvedMetadata.templateUrl);
        this.html = resolvedMetadata.htmlAst.rootNodes.map((v)=> nodeFactory(v));
    }

    public toJSON(): any {

        const { url, html } = this;

        return {
            kind: 'template',
            url,
            html,
        };
    }
}

