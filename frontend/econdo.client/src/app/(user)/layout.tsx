'use client';

import { ReactNode } from "react";
import ProfileProvider from "@/providers/profileProvider";

export default function UserLayout(
    {children} : {children: ReactNode}) {
    
    return (
        <ProfileProvider>
            {children}
        </ProfileProvider>
    );
}