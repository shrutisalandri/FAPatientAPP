export class User {
    userID: number;
    name: string;
    email: string;
    password: string;
    isLocked: boolean = false;
    isActive: boolean = true;
    status: string;
    accessToken: string;
    roleName: string;
}
