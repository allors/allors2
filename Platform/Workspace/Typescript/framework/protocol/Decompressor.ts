import { Compressor } from './Compressor';
import { MetaPopulation } from '../meta/MetaPopulation';

export const createMetaDecompressor = (decompressor: Decompressor, metaPopulation: MetaPopulation) => (compressed: string) => {
  return metaPopulation.metaObjectById[decompressor.read(compressed, v => {})];
}

export class Decompressor {
  private valueByKey: { [k: string]: string };

  constructor() {
    this.valueByKey = {};
  }

  public read(compressed: string, first: (value: string) => void): string {

    if (compressed !== undefined && compressed !== null) {
      if (compressed[0] === Compressor.indexMarker) {
        const secondIndexMarkerIndex = compressed.indexOf(Compressor.indexMarker, 1);
        const key = compressed.slice(1, secondIndexMarkerIndex - 1);
        const value = compressed.slice(secondIndexMarkerIndex + 1);
        this.valueByKey[key] = value;
        first(value);
        return value;
      }

      return this.valueByKey[compressed];
    }

    return null;
  }
}
