import { accessTokenCookieKey, refreshTokenCookieKey } from "@/utils/constants";
import { cookies } from "next/headers";
import { NextRequest, NextResponse } from "next/server";
import { generateAccessToken, logout, setAccessTokenCookie } from "./actions/auth";
import { jwtDecode } from "jwt-decode";
import { updateCacheableAuthInstanceInterceptor } from "./lib/axiosInstance";

const protectedRoutes = [
    '/condos/buildings', 
    '/condos/properties', 
    '/logout', 
    '/profile'
];

const publicRoutes = ['/login', '/register', '/'];

export default async function middleware(req: NextRequest) {
    const path = req.nextUrl.pathname;

    const cookieStore = (await cookies());
    let accessToken = cookieStore.get(accessTokenCookieKey)?.value;
    const refreshTokenCookie = cookieStore.get(refreshTokenCookieKey);
    if(accessToken) {
        const token = jwtDecode(accessToken);

        if(token.exp! < Date.now() / 1000)
            accessToken = undefined;
    }
    
    if(accessToken && !refreshTokenCookie) {
        cookieStore.delete(accessTokenCookieKey);
        return NextResponse.redirect(new URL('/', req.nextUrl));
    }

    if(!accessToken && refreshTokenCookie) {
        const res = await generateAccessToken();
        if(res.ok) {
            await setAccessTokenCookie(res.value!.accessToken, 
                res.value!.expiresIn);
            accessToken = res.value!.accessToken;

            // updateCacheableAuthInstanceInterceptor(await cookies());
        } else {
            cookieStore.delete(refreshTokenCookieKey);
            cookieStore.delete(accessTokenCookieKey);
            return NextResponse.redirect(new URL('/', req.nextUrl));
        }

        return NextResponse.redirect(req.nextUrl);
    }
    
    if(path.startsWith('/logout')) {
        try {
            await logout();
            return NextResponse.redirect(new URL('/', req.nextUrl));
        }
        catch(error) {
            console.error(error);
        }
    }

    if(protectedRoutes.includes(path) && !accessToken)
        return NextResponse.redirect(new URL('/login', req.nextUrl));

    if(publicRoutes.includes(path) && accessToken && !path.startsWith('/condos')) {
        return NextResponse.redirect(new URL('/condos/properties', req.nextUrl));
    }

    return NextResponse.next();
}
