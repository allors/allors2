import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class TypeOfExpression implements Expression {
    constructor(statement: ts.TypeOfExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'typeOfExpression',
        };
    }
}

