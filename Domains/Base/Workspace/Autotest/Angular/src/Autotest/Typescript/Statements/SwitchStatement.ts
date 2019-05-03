import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class SwitchStatement implements Statement {
    constructor(statement: ts.SwitchStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'switchStatement',
        };
    }
}

