import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class PostfixUnaryExpression implements Expression {
    constructor(statement: ts.PostfixUnaryExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'postfixUnaryExpression',
        };
    }
}

