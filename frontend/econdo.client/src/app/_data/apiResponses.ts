export interface TokenResponse {
    accessToken: string;
    expiresIn: number;
    refreshToken: string;
};

export interface ApiError {
    status: number;
    title: string;
    errors: Map<string, string[]>;
}