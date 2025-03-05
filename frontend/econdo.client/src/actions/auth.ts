'use server';
import axios, { isAxiosError } from 'axios';
import { ValidationError, TokenResponse } from '@/app/_data/apiResponses';
import { cookies } from 'next/headers';
import { accessTokenCookieKey, refreshTokenCookieKey } from '@/utils/constants';
import { LoginData } from '@/app/_data/loginData';
import authInstance, { normalInstance } from '@/lib/axiosInstance';
import { RegisterData } from '@/app/_data/registerData';
import { Result, resultFail, resultOk } from '@/utils/result';

const backendApiUrl = process.env.NEXT_PRIVATE_BACKEND_URL;

export async function login(loginData: LoginData): Promise<ValidationError | null> {
    try {
        const res = await normalInstance.post<TokenResponse>('/api/account/login', loginData)
        const cookieStore = await cookies();
    
        cookieStore.set(accessTokenCookieKey, res.data.accessToken, { httpOnly: true, sameSite: 'strict', maxAge: res.data.expiresIn * 60});
        cookieStore.set(refreshTokenCookieKey, res.data.refreshToken, { httpOnly: true, sameSite: 'strict' });
    } catch(error) {
        if(isAxiosError<ValidationError, Record<string, unknown>>(error)) {
            return error.response?.data!;
        }

        console.error(error);
    }

    return null;
}

const baseUrl = process.env.NEXT_PRIVATE_BASE_URL;

export async function register(registerData: RegisterData) : Promise<ValidationError | null> {
    try {
        await normalInstance.post('/api/account/register', {
            email: registerData.email,
            username: registerData.username,
            password: registerData.password,
            returnUri: `${baseUrl}/confirmAccount`,
        });
    } catch(error) {
        if(!isAxiosError(error)) {
            console.error(error);
            return null;
        }

        if(isAxiosError<ValidationError, Record<string, unknown>>(error))
            return error.response?.data!;
    }

    return null;
}

export async function generateAccessToken(): Promise<Result<TokenResponse, ValidationError | Error>> {
    try {
        const cookieStore = await cookies();
        
        const res = await axios.post<TokenResponse>(`${backendApiUrl}/api/account/refresh`, {
            refreshToken: cookieStore.get(refreshTokenCookieKey)?.value,
        })
    
        return resultOk(res.data);
    } catch(error) {
        if(isAxiosError<ValidationError, Record<string, unknown>>(error))
            return resultFail<TokenResponse, ValidationError>(error.response?.data!);
    }

    return resultFail(new Error("Failed to generate new access token"));
}

export async function setAccessTokenCookie(accessToken: string, maxAge: number) {
    const cookieStore = await cookies();
    cookieStore.set(accessTokenCookieKey, accessToken, { httpOnly: true, sameSite: 'strict', maxAge: maxAge * 60});
}

export async function confirmEmail(token: string, email: string): Promise<void> {
    return await normalInstance.post('/api/account/confirmEmail', {
        token: token,
        email: email,
    });
}

export async function isAuthenticated(): Promise<boolean> {
    const cookieStore = await cookies();
    return cookieStore.get(accessTokenCookieKey) !== undefined;
}

export async function logout() {
    const cookieStore = await cookies();
    await authInstance.post('/api/account/invalidateRefreshToken', {
        refreshToken: cookieStore.get(refreshTokenCookieKey)?.value,
    });

    cookieStore.delete(refreshTokenCookieKey);
    cookieStore.delete(accessTokenCookieKey);
}
