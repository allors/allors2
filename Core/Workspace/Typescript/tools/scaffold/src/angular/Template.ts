import { CompileTemplateMetadata } from '@angular/compiler';
import { PathResolver } from '../Helpers';
import { Node, nodeFactory } from '../Html/Node';

import { Directive } from './Directive';

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

