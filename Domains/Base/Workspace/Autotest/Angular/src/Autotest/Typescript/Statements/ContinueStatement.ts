import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class ContinueStatement implements Statement {
    constructor(statement: ts.ContinueStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'continueStatement',
        };
    }
}

