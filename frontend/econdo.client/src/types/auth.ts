export interface RegisterData {
    email: string;
    username: string;
    password: string;
}

export interface LoginData {
    email: string;
    password: string;
}

export interface TokenResult {
    accessToken: string;
    expiresIn: number;
    refreshToken: string;
}

export interface AccessTokenResult {
    accessToken: string;
}

export type AuthEvent = 'confirmAccount' | 'forgotPassword' | 'resetPassword' | 'accountVerified';

export const accountVerifiedEvent: AuthEvent = 'accountVerified';
export const confirmAccountEvent: AuthEvent = 'confirmAccount';
export const forgotPasswordEvent: AuthEvent = 'forgotPassword';
export const resetPasswordEvent: AuthEvent = 'resetPassword';

