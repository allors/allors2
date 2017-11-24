import { AutomatedAgent } from "@allors/generated/dist/domain/AutomatedAgent.g";

declare module "@allors/generated/dist/domain/AutomatedAgent.g" {
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
