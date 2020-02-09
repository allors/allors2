import { User } from '../generated/User.g';

declare module '../generated/User.g' {
    interface User {
        displayName: string;

    }
}
