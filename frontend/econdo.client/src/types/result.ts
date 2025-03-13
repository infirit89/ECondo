export type Result<T = void, E extends Error = Error> =
    | { ok: true, value?: T }
    | { ok: false, error: E };

export function resultOk<T = void, E extends Error = Error>(value?: T): Result<T, E> {
    return { ok: true, value: value };
}

export function resultFail<T = void, E extends Error = Error>(error: E): Result<T, E> {
    return { ok: false, error: error };
}
