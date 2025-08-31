import { generateAccessToken, isUserInRole, setAccessTokenCookie } from '@/actions/auth';
import { accessTokenCookieKey, refreshTokenCookieKey } from '@/utils/constants';
import { jwtDecode } from 'jwt-decode';
import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';
import { cache } from 'react';
import 'server-only';

enum SessionState {
    Valid,  // both the refresh and acess token are present and valid
    NeedsRefresh,   // the access token needs to be invalidated
    Invalid // there is something wrong with the session
}

export const getSessionState = async (): Promise<SessionState> => {
    const cookieStore = (await cookies());
    const accessTokenCookie = cookieStore.get(accessTokenCookieKey);
    const refreshTokenCookie = cookieStore.get(refreshTokenCookieKey);

    if (refreshTokenCookie) {
        if (accessTokenCookie) {

            const token = jwtDecode(accessTokenCookie.value);

            if (token.exp! < Date.now() / 1000)
                return SessionState.NeedsRefresh;

            return SessionState.Valid;
        }
        else
            return SessionState.NeedsRefresh;
    }

    return SessionState.Invalid;
};

export const verifySession = cache(async () => {
    const cookieStore = (await cookies());
    const sessionState = await getSessionState();
    switch (sessionState) {
        case SessionState.Valid:
            return;
        case SessionState.NeedsRefresh:
            const res = await generateAccessToken();
            if (!res.ok) {
                cookieStore.delete(refreshTokenCookieKey);
                cookieStore.delete(accessTokenCookieKey);
                return redirect('/');
            }

            await setAccessTokenCookie(res.value!.accessToken,
                res.value!.expiresIn);
        case SessionState.Invalid:
            cookieStore.delete(refreshTokenCookieKey);
            cookieStore.delete(accessTokenCookieKey);
            return redirect('/');
    }
});

export const isAdmin = cache(async () => {
    verifySession();
    return (await isUserInRole('admin')).ok;
})
