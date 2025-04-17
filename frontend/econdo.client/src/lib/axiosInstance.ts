import { accessTokenCookieKey } from "@/utils/constants";
import axios from "axios";
import { ReadonlyRequestCookies } from "next/dist/server/web/spec-extension/adapters/request-cookies";
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
        const accessToken = cookieStore.get(accessTokenCookieKey)?.value;
        config.headers.Authorization = `Bearer ${accessToken}`;
        return config;
    },
    (error) => Promise.reject(error),
);

export const cacheableAuthInstance = axios.create({
    baseURL: `${backendApiUrl}`,
});

let cacheableAuthInstanceAccessTokenInterceptor: 
    number | undefined = undefined;

export function updateCacheableAuthInstanceInterceptor(
    cookieStore: ReadonlyRequestCookies) {

    if(cacheableAuthInstanceAccessTokenInterceptor) {
        cacheableAuthInstance
            .interceptors
                .request
                    .eject(cacheableAuthInstanceAccessTokenInterceptor);
    }

    console.log('aaaaaaaaaaaaaaaaaaaaaaaa');
    cacheableAuthInstanceAccessTokenInterceptor = 
        cacheableAuthInstance.interceptors.request.use(
            async (config) => {
                if(config.headers.Authorization)
                    return config;
        
                const accessToken = cookieStore.get(accessTokenCookieKey)?.value;
                console.log('aaaaaaaaaaaa', accessToken);
                config.headers.Authorization = `Bearer ${accessToken}`;
                return config;
            },
            (error) => Promise.reject(error),
        );
}

export default authInstance;