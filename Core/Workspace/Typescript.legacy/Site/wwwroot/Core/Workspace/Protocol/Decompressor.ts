namespace Allors.Protocol {
  import MetaPopulation = Meta.MetaPopulation;

  export const createMetaDecompressor = (decompressor: Decompressor, metaPopulation: MetaPopulation) => (compressed: string) => {
        return metaPopulation.metaObjectById.get(decompressor.read(compressed, v => { }));
    };

    export class Decompressor {
        private valueByKey: Map<string, string>;

        constructor() {
            this.valueByKey = new Map();
        }

        public read(compressed: string, first: (value: string) => void): string {

            if (compressed !== undefined && compressed !== null) {
                if (compressed[0] === Compressor.indexMarker) {
                    const secondIndexMarkerIndex = compressed.indexOf(Compressor.indexMarker, 1);
                    const key = compressed.slice(1, secondIndexMarkerIndex);
                    const value = compressed.slice(secondIndexMarkerIndex + 1);
                    this.valueByKey.set(key, value);
                    first(value);
                    return value;
                }

                return this.valueByKey.get(compressed);
            }

            return null;
        }
    }
}
