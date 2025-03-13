export interface ValidationError extends Error {
    title: string; 
    errors: Record<string, string[]>;
}

export function isValidationError(error: any): error is ValidationError {
    if(error === null)
        return false;

    return 'errors' in error;
}

export interface ApiResponse {
    status?: number;
}
