'use server';
import axios from 'axios';
import { ApiError, ApiSucess, isApiError, TokenResponse } from '@/app/_data/apiResponses';
import { cookies } from 'next/headers';
import { accessTokenCookieKey, refreshTokenCookieKey } from '@/utils/constants';
import { axiosToApiErrorConverter } from '@/utils/helper';
import { LoginData } from '@/app/_data/loginData';
import { normalInstance } from '@/lib/axiosInstance';
import { RegisterData } from '@/app/_data/registerData';

const backendApiUrl = process.env.NEXT_PRIVATE_BACKEND_URL;

export async function login(loginData: LoginData): Promise<ApiError | TokenResponse> {
    const res = await normalInstance.post<TokenResponse>('/api/account/login', loginData)
    .catch(axiosToApiErrorConverter);
    
    if(isApiError(res))
        return res;

    const cookieStore = await cookies();

    cookieStore.set(accessTokenCookieKey, res.data.accessToken, { httpOnly: true, sameSite: 'strict', maxAge: res.data.expiresIn * 60});
    cookieStore.set(refreshTokenCookieKey, res.data.refreshToken, { httpOnly: true, sameSite: 'strict' });

    return res.data;
}

export async function register(registerData: RegisterData): Promise<ApiError | ApiSucess> {
    const res = await normalInstance.post('/api/account/register', registerData)
    .catch(axiosToApiErrorConverter);

    if(isApiError(res))
        return res;

    return {
        status: res.status,
        statusText: res.statusText,
    };
}

export async function generateAccessToken(): Promise<ApiError | TokenResponse> {
    const cookieStore = await cookies();
    
    const res = await axios.post<TokenResponse>(`${backendApiUrl}/api/account/refresh`, {
        refreshToken: cookieStore.get(refreshTokenCookieKey)?.value,
    })
    .catch(axiosToApiErrorConverter);

    if(isApiError(res))
        return res;

    return res.data;
}

export async function setAccessTokenCookie(accessToken: string, maxAge: number) {
    const cookieStore = await cookies();
    cookieStore.set(accessTokenCookieKey, accessToken, { httpOnly: true, sameSite: 'strict', maxAge: maxAge * 60});
}

export async function confirmEmail(token: string, email: string): Promise<ApiError | ApiSucess> {
    const res = await normalInstance.post('/api/account/confirmEmail', {
        token: token,
        email: email,
    })
    .catch(axiosToApiErrorConverter);

    if(isApiError(res))
        return res;

    return {
        status: res.status,
        statusText: res.statusText,
    };
}
