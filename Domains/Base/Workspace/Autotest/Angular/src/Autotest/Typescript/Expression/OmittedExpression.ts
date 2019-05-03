import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class OmittedExpression implements Expression {
    constructor(statement: ts.OmittedExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'omittedExpression',
        };
    }
}

