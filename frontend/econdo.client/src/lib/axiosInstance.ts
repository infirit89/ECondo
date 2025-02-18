import { accessTokenCookieKey } from "@/utils/constants";
import axios from "axios";
import { cookies } from "next/headers";

const backendApiUrl = process.env.NEXT_PRIVATE_BACKEND_URL;

export const normalInstance = axios.create({
    baseURL: `${backendApiUrl}`,
});

const authInstance = axios.create({
    baseURL: `${backendApiUrl}`,
});

authInstance.interceptors.request.use(
    async (config) => {
        if(config.headers.Authorization)
            return config;

        const cookieStore = await cookies();
        config.headers.Authorization = `Bearer ${cookieStore.get(accessTokenCookieKey)?.value}`;
        return config;
    },
    (error) => Promise.reject(error),
);

export default authInstance;