export interface TokenResponse {
    accessToken: string;
    expiresIn: number;
    refreshToken: string;
};

export interface AccessTokenResponse {
    accessToken: string;
}

export interface ApiError {
    status: number;
    title: string;
    errors: Map<string, string[]>;
}

export function isApiError(response : ApiError | any): response is ApiError {
    return 'errors' in response;
}