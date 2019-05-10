import * as compiler from '@angular/compiler';

import { Element } from './Element';
import { Text } from './Text';
import { Attribute } from './Attribute';
import { Comment } from './Comment';

export function nodeFactory(node: compiler.Node): Node {
    if(node instanceof compiler.Element){
        return new Element(node as compiler.Element);
    }

    if(node instanceof compiler.Text){
        return new Text(node as compiler.Text);
    }

    if(node instanceof compiler.Attribute){
        return new Attribute(node as compiler.Attribute);
    }

    if(node instanceof compiler.Comment){
        return new Comment(node as compiler.Comment);
    }
}

export interface Node {
}

