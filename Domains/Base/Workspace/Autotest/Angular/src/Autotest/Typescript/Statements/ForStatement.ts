import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class ForStatement implements Statement {
    constructor(statement: ts.ForStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'forStatement',
        };
    }
}

