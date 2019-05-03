import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class ReturnStatement implements Statement {
    constructor(statement: ts.ReturnStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'returnStatement',
        };
    }
}

