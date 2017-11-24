import { User } from "@allors/generated/dist/domain/user.g";

declare module "@allors/generated/dist/domain/User.g" {
    interface User {
        displayName: string;

    }
}
