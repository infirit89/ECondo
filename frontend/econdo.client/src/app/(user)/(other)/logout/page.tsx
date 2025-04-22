'use client';

import { logout } from "@/actions/auth";
import { userProfileSessionKey } from "@/utils/constants";
import { redirect } from "next/navigation";
import { useEffect } from "react";

export default function LogoutPage() {

    useEffect(() => {
        sessionStorage.removeItem(userProfileSessionKey);

        logout();
        redirect('/');
    }, []);
}