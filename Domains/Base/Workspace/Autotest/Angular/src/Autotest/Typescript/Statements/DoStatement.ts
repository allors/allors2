import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class DoStatement implements Statement {
    constructor(statement: ts.DoStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'doStatement',
        };
    }
}

