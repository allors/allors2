import { AutomatedAgent } from "@generatedDomain/AutomatedAgent.g";

declare module "@generatedDomain/AutomatedAgent.g" {
    interface AutomatedAgent {
        displayName: string;

    }
}

Object.defineProperty(AutomatedAgent.prototype, "displayName", {
  get(this: AutomatedAgent): string {
    if (this.UserName) {
        return this.UserName;
    }

    return "N/A";
  },
});
