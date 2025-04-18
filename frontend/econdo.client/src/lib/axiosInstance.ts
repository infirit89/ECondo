'use server';

import axios from "axios";

const backendApiUrl = process.env.NEXT_PRIVATE_BACKEND_URL;

export const normalInstance = axios.create({
    baseURL: `${backendApiUrl}`,
});

const authInstance = axios.create({
    baseURL: `${backendApiUrl}`,
});

export async function setAccessToken(accessToken: string) {
    authInstance.defaults.headers.common.Authorization = `Bearer ${accessToken}`;
}


export default authInstance;