import { Symbol } from "../ngast/symbol";
import { PathResolver } from '../Helpers';

export type StaticSymbol = {
    name: string,
    filePath: string,
}

export class Reference {

    name: string;
    path: string;

    constructor(public symbol: StaticSymbol, pathResolver: PathResolver) {

        this.name = symbol.name;
        this.path = pathResolver.relative(symbol.filePath);
    }

    static fromSymbol(symbol: Symbol | StaticSymbol, pathResolver: PathResolver): Reference {
        if (symbol) {
            if ((symbol as StaticSymbol).name && (symbol as StaticSymbol).filePath) {
                return new Reference((symbol as StaticSymbol), pathResolver);
            }

            return new Reference((symbol as Symbol).symbol, pathResolver);
        }

        return undefined;
    }

    static fromSymbols(symbols: (Symbol | StaticSymbol)[], pathResolver: PathResolver): Reference[] {
        return symbols && symbols.length ? symbols.map((v) => this.fromSymbol(v, pathResolver)).filter((v) => v) : undefined;
    }

    static fromValue(value: any, pathResolver: PathResolver): any {
        if (value) {

            if (value.name && value.filePath) {
                return this.fromSymbol(value, pathResolver);
            }

            var clone = JSON.parse(JSON.stringify(value))
            Reference.replace(clone, pathResolver);
            return clone;
        }

        return undefined;
    }

    private static replace(clone: any, pathResolver: PathResolver) {
        for (const key of Object.keys(clone)) {
            var value = clone[key];
            if (value.name && value.filePath) {
                clone[key] = this.fromSymbol(value, pathResolver);
            } else {
                if (Array.isArray(value)) {
                    for (let i = 0; i < value.length; i++) {
                        const element = value[i];
                        if (element.name && element.filePath) {
                            value[i] = new Reference(element, pathResolver);
                        } else if (element === Object(element)) {
                            Reference.replace(element, pathResolver);
                        }
                    }
                } else if (value === Object(value)) {
                    Reference.replace(value, pathResolver);
                }
            }
        }
    }

    public toJSON(): any {

        const { name, path } = this;

        return {
            kind: 'reference',
            name,
            path,
        };
    }
}

