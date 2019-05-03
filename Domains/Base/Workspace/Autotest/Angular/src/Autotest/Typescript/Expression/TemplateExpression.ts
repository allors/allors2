import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class TemplateExpression implements Expression {
    constructor(statement: ts.TemplateExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'templateExpression',
        };
    }
}

