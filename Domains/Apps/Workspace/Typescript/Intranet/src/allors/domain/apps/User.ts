import { domain } from "../domain";
import { User } from "../generated/user.g";

declare module "../generated/User.g" {
    interface User {
        displayName: string;

    }
}
