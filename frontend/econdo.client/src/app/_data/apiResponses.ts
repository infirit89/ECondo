import { AxiosError } from "axios";

export interface TokenResponse {
    accessToken: string;
    expiresIn: number;
    refreshToken: string;
}

export interface AccessTokenResponse {
    accessToken: string;
}

export interface ValidationError extends Error {
    title: string; 
    errors: Record<string, string[]>;
}

export function isValidationError(response: any): response is ValidationError {
    if(response === null)
        return false;
    
    return 'errors' in response;
}

export interface ApiResponse {
    status?: number;
}