import { Directive } from './Directive';
import { Node, nodeFactory } from './Html/Node';

export class Template {

    url: string;
    html: Node[];

    constructor(public directive: Directive) {
        const resolvedMetadata = this.directive.resolvedMetadata;

        this.url = resolvedMetadata.templateUrl;
        this.html = resolvedMetadata.htmlAst.rootNodes.map((v)=> nodeFactory(v));
    }

    public toJSON(): any {

        const { url, html } = this;

        return {
            url,
            html,
        };
    }
}

