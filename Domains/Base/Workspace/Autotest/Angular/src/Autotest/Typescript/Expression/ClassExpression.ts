import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class ClassExpression implements Expression {
    constructor(statement: ts.ClassExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'classExpression',
        };
    }
}

