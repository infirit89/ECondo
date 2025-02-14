import { TokenResponse } from "@/app/_data/apiResponses";
import { LoginFields } from "@/app/_data/loginData";
import axios from "axios";
import { NextResponse } from "next/server";

export async function POST(req: Request) {
    const loginData: LoginFields = await req.json() as LoginFields;
    axios.post<TokenResponse>(`${process.env.NEXT_PRIVATE_BACKEND_URL}/api/account/login`, loginData);
    return NextResponse.next({ status: 200 });
}