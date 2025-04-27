'use server';

import axios, { isAxiosError } from 'axios';
import { ApiError } from '@/types/apiResponses';
import { cookies } from 'next/headers';
import { 
    accessTokenCookieKey,
    refreshTokenCookieKey 
} from '@/utils/constants';
import normalInstance, { authInstance } from '@/lib/axiosInstance';
import { RegisterData, LoginData, TokenResponse } from '@/types/auth';
import { Result, resultFail, resultOk } from '@/types/result';
import { cache } from 'react';

const backendApiUrl = process.env.NEXT_PRIVATE_BACKEND_URL;

export async function login(loginData: LoginData): Promise<Result> {
    
    try {
        const res = await normalInstance
            .post<TokenResponse>('/api/account/login', loginData)
        const cookieStore = await cookies();
    
        cookieStore.set(
            accessTokenCookieKey,
            res.data.accessToken,
            {
                httpOnly: true,
                sameSite: 'strict',
                maxAge: res.data.expiresIn * 60
            });
        cookieStore.set(
            refreshTokenCookieKey,
            res.data.refreshToken,
            {
                httpOnly: true,
                sameSite: 'strict'
            });

        return resultOk();
    } catch(error) {
        if(isAxiosError<ApiError>(error) && error.response?.data)
            return resultFail(error.response?.data);
    }

    throw new Error('Unexpected code flow');
}

const baseUrl = process.env.NEXT_PRIVATE_BASE_URL;

export async function register(registerData: RegisterData): Promise<Result> {
    try {
        await normalInstance.post('/api/account/register', {
            email: registerData.email,
            username: registerData.username,
            password: registerData.password,
            returnUri: `${baseUrl}/confirmAccount`,
        });

        return resultOk();
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
}

export async function generateAccessToken(): Promise<Result<TokenResponse>> {
    try {
        const cookieStore = await cookies();
        
        const res = await axios.
            post<TokenResponse>(`${backendApiUrl}/api/account/refresh`, 
            {
                refreshToken: cookieStore
                    .get(refreshTokenCookieKey)?.value,
            });
    
        return resultOk(res.data);
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
}

export async function setAccessTokenCookie(accessToken: string, maxAge: number) {
    const cookieStore = await cookies();
    cookieStore.set(
        accessTokenCookieKey,
        accessToken,
        {
            httpOnly: true,
            sameSite: 'strict',
            maxAge: maxAge * 60
        });
}

export async function confirmEmail(token: string, email: string): Promise<Result> {
    try {
        await normalInstance.post('/api/account/confirmEmail', {
            token: token,
            email: email,
        });

        return resultOk();
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);   
    }

    throw new Error('Unexpected code flow');
}

export async function logout() {
    const cookieStore = await cookies();
    const refreshToken = cookieStore.get(refreshTokenCookieKey)?.value;
    
    await authInstance.post('/api/account/invalidateRefreshToken', {
        refreshToken: refreshToken,
    });

    cookieStore.delete(refreshTokenCookieKey);
    cookieStore.delete(accessTokenCookieKey);
}

export async function forgotPassword(email: string): Promise<Result> {
    try {
        await normalInstance.post('/api/account/forgotPassword', {
            username: email,
            returnUri: `${baseUrl}/resetPassword`,
        });

        return resultOk();
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
}

export async function resetPassword(email: string, token: string, password: string): Promise<Result> {
    try {
        await normalInstance.post('/api/account/resetPassword', {
            email: email,
            token: token,
            newPassword: password,
        });

        return resultOk();
    }
    catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
}

export async function updatePassword(currentPassword: string, newPassword: string): Promise<Result> {
    
    try {
        await authInstance.put('/api/account/updatePassword', {
            currentPassword: currentPassword,
            newPassword: newPassword
        });

        return resultOk();
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
}

export const isUserInRole =
cache(async (roleName: string): Promise<Result> => {
    try {
        await authInstance.get('/api/account/isInRole', {
            params: {
                roleName
            },
        });

        return resultOk();
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
})
