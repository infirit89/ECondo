import { ApiError } from "./apiResponses";

export type Result<T = void, E = ApiError> =
    | { ok: true, value?: T }
    | { ok: false, error: E };

export function resultOk<T = void, E = ApiError>(value?: T): Result<T, E> {
    return { ok: true, value: value };
}

export function resultFail<T = void, E = ApiError>(error: E): Result<T, E> {
    return { ok: false, error: error };
}
