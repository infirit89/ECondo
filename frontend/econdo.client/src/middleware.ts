import { accessTokenCookieKey, refreshTokenCookieKey } from "@/utils/constants";
import { cookies } from "next/headers";
import { NextRequest, NextResponse } from "next/server";
import { generateAccessToken, setAccessTokenCookie } from "./actions/auth";
import { isApiError } from "./app/_data/apiResponses";

const protectedRoutes = ['/dashboard'];
const publicRoutes = ['/login', '/signup', '/'];

export default async function middleware(req: NextRequest) {
    const path = req.nextUrl.pathname;

    const cookieStore = (await cookies());
    let accessToken = cookieStore.get(accessTokenCookieKey)?.value;
    const refreshTokenCookie = cookieStore.get(refreshTokenCookieKey);
    if(!accessToken && refreshTokenCookie) {
        const res = await generateAccessToken();
        if(!isApiError(res)) {
            await setAccessTokenCookie(res.accessToken, res.expiresIn * 60);
            accessToken = res.accessToken;
        }
    }
    
    if(protectedRoutes.includes(path) && !accessToken)
        return NextResponse.redirect(new URL('/login', req.nextUrl));

    if(publicRoutes.includes(path) && accessToken && !path.startsWith('/dashboard')) {
        return NextResponse.redirect(new URL('/dashboard', req.nextUrl));
    }

    return NextResponse.next();
}
