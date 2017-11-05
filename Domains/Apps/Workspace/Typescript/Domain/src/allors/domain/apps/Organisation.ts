import { Organisation } from "@generatedDomain/Organisation.g";

declare module "@generatedDomain/Organisation.g" {
    interface Organisation {
        displayName: string;

    }
}

Object.defineProperty(Organisation.prototype, "displayName", {
  get(this: Organisation): string {

    if (this.Name) {
        return this.Name;
    }

    return "N/A";
  },
});
