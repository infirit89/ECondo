export interface RegisterData {
    email: string;
    username: string;
    password: string;
}

export interface LoginData {
    email: string;
    password: string;
}

export interface TokenResponse {
    accessToken: string;
    expiresIn: number;
    refreshToken: string;
}

export interface AccessTokenResponse {
    accessToken: string;
}

export type AuthEvent = 'confirmAccount' | 'forgotPassword' | 'resetPassword';
export const confirmAccountEvent: AuthEvent = 'confirmAccount';
export const forgotPasswordEvent: AuthEvent = 'forgotPassword';
export const resetPasswordEvent: AuthEvent = 'resetPassword';

