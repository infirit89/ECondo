'use server';

import { accessTokenCookieKey } from "@/utils/constants";
import axios from "axios";
import { cookies } from "next/headers";

const backendApiUrl = process.env.NEXT_PRIVATE_BACKEND_URL;

const normalInstance = axios.create({
    baseURL: `${backendApiUrl}`,
});

export const authInstance = axios.create({
    baseURL: `${backendApiUrl}`,
});

authInstance.interceptors.request.use(
    async (config) => {
        if(config.headers.Authorization)
            return config;

        const cookieStore = await cookies();
        const accessToken = cookieStore.get(accessTokenCookieKey)?.value;
        config.headers.Authorization = `Bearer ${accessToken}`;
        return config;
    },
    (error) => Promise.reject(error),
);

export default normalInstance;