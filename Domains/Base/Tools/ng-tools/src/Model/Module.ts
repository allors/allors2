import { ModuleSymbol } from "ngast";

export class Module {
    get name() { return this.symbol.symbol.name; }

    get isLocal() { return this.symbol.symbol.filePath.indexOf('node_modules') === -1; }

    constructor(public symbol: ModuleSymbol) {
    }
}

