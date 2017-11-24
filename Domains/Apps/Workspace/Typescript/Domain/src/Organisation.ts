import { Organisation } from "@allors/generated/dist/domain/Organisation.g";

declare module "@allors/generated/dist/domain/Organisation.g" {
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
