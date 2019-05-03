import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class BinaryExpression implements Expression {
    constructor(statement: ts.BinaryExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'binaryExpression',
        };
    }
}

