namespace Allors.Protocol {
  export class Compressor {
    static readonly indexMarker = '~';
    static readonly itemSeparator = ',';

    private keyByValue: { [k: string]: string };
    private counter: number;

    constructor() {
      this.keyByValue = {};
      this.counter = 0;
    }

    public write(value: string): string {
      if (value === undefined || value === null) {
        return null;
      }

      if (this.keyByValue.hasOwnProperty(value)) {
        return this.keyByValue[value];
      }

      const key = (++this.counter).toString();
      this.keyByValue[value] = key;
      return `${Compressor.indexMarker}${key}${Compressor.indexMarker}${value}`;
    }

  }
}
