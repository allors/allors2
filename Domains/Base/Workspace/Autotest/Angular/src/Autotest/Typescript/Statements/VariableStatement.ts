import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class VariableStatement implements Statement {
    constructor(statement: ts.VariableStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'variableStatement',
        };
    }
}

