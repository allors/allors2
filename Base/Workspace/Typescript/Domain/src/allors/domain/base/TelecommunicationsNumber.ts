import { domain } from '../domain';
import { TelecommunicationsNumber } from '../generated/TelecommunicationsNumber.g';
import { Meta } from '../../meta/generated/domain.g';

declare module '../generated/TelecommunicationsNumber.g' {
  interface TelecommunicationsNumber {
    displayName;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const obj = workspace.constructorByObjectType.get(m.TelecommunicationsNumber).prototype as any;

  Object.defineProperty(obj, 'displayName', {
    configurable: true,
    get(this: TelecommunicationsNumber) {
      let numberString = '';
      if (this.CountryCode || this.AreaCode) {
        if (this.CountryCode && this.AreaCode) {
          numberString = this.CountryCode + ' ' + this.AreaCode;
        } else if (this.CountryCode) {
          numberString = this.CountryCode;
        } else {
          numberString = this.AreaCode;
        }
      }

      if (numberString === '' && this.ContactNumber) {
        return numberString = this.ContactNumber;
      } else {
        return numberString += ' ' + this.ContactNumber;
      }
    },
  });

});
