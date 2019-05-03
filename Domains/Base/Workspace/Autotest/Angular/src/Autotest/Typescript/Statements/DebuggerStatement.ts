import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class DebuggerStatement implements Statement {
    constructor(statement: ts.DebuggerStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'debuggerStatement',
        };
    }
}

