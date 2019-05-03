import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class WhileStatement implements Statement {
    constructor(statement: ts.WhileStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'whileStatement',
        };
    }
}

