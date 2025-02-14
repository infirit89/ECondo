'use server';

import { LoginFields } from '@/app/_data/loginData';
import axios, { AxiosError } from 'axios';
import { ApiError, TokenResponse } from './_data/apiResponses';
import { cookies } from 'next/headers';

const backendApiUrl = process.env.NEXT_PRIVATE_BACKEND_URL;

export async function login(loginData: LoginFields): Promise<ApiError | TokenResponse> {
    const res =  await axios.post<TokenResponse>(`${backendApiUrl}/api/account/login`, loginData)
    .catch((ex: AxiosError) : ApiError => {
        let apiError = ex.response?.data as ApiError;
        apiError.errors = new Map(Object.entries(apiError.errors));
        return apiError;
    });

    if('errors' in res)
        return res;

    return res.data;
}

export async function setAuthCookie({accessToken, maxAge , refreshToken} : { accessToken : string, maxAge: number, refreshToken: string }) {
    const cookieStore = await cookies();

    cookieStore.set('ecota', accessToken, { httpOnly: true, sameSite: 'strict', maxAge: maxAge});
    cookieStore.set('ecotr', refreshToken, { httpOnly: true, path: '/user', sameSite: 'strict' });
}

export async function generateAccessToken() {
    
}