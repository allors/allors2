import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class ExpressionWithTypeArguments implements Expression {
    constructor(statement: ts.ExpressionWithTypeArguments, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'expressionWithTypeArguments',
        };
    }
}

