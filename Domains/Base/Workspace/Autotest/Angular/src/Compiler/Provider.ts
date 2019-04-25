import { ProviderSymbol } from "ngast";

export class Provider {

    get tokenIdentifier() { return this.symbol.getMetadata().token.identifier; }

    get tokenValue() { return this.symbol.getMetadata().token.value; }

    get useValue() { return this.symbol.getMetadata().useValue }

    constructor(public symbol: ProviderSymbol) {
    }

    public toJSON(): any {

        const { tokenIdentifier, tokenValue, useValue } = this;

        const tokenReference = tokenIdentifier && tokenIdentifier.reference;

        return {
            tokenReference,
            tokenValue,
            useValue,
        };
    }

}

