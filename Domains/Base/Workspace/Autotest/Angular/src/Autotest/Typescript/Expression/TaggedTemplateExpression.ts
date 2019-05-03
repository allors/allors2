import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class TaggedTemplateExpression implements Expression {
    constructor(statement: ts.TaggedTemplateExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'taggedTemplateExpression',
        };
    }
}

