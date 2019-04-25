import { PipeSymbol } from "ngast";

export class Pipe {
    get name() { return this.symbol.symbol.name; }

    get isLocal() { return this.symbol.symbol.filePath.indexOf('node_modules') === -1 }

    constructor(public symbol: PipeSymbol) {
    }

    
    public toJSON(): any {

        const { name, isLocal } = this;

        return {
            name,
            isLocal,
        };
    }
}

