import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class VoidExpression implements Expression {
    constructor(statement: ts.VoidExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'voidExpression',
        };
    }
}

