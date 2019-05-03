import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class PrefixUnaryExpression implements Expression {
    constructor(statement: ts.PrefixUnaryExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'prefixUnaryExpression',
        };
    }
}

