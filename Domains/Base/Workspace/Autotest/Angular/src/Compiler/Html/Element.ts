import * as compiler from '@angular/compiler';

import { Node, nodeFactory } from './Node';
import { Attribute } from './Attribute';

export class Element implements Node {

    name: string;

    children: Node[];

    attributes: Attribute[];

    constructor(public node: compiler.Element) {

        this.name = node.name;
        this.children = node.children.map((v) => nodeFactory(v));
        this.attributes = node.attrs.map((v) => new Attribute(v));
    }

    public toJSON(): any {

        const { name, children, attributes } = this;

        return {
            kind: 'element',
            name,
            children,
            attributes,
        };
    }
}

