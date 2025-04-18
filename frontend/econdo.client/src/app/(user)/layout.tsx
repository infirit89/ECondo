'use server';

import { updateAccessToken } from "@/actions/auth";
import { getBriefProfile } from "@/actions/profile";
import AppProvider from "@/components/appProvider/appProvider";
import { UserNavbar } from "@/components/navbar/userNavbar";
import { AppShell, AppShellFooter, AppShellMain } from "@mantine/core";
import { ReactNode, Suspense } from "react";

export default async function UserLayout( { children }: 
    Readonly<{ children: ReactNode }> ) {

    await updateAccessToken();
    const profileRes = await getBriefProfile();

    if(!profileRes.ok)
        throw new Error();

    return (
        <AppProvider profileData={profileRes.value}>
            <AppShell 
            header={{ height: 60 }}>
                <UserNavbar 
                {...profileRes.value} />
                <AppShellMain>
                    <Suspense>
                        {children}
                    </Suspense>
                </AppShellMain>
                <AppShellFooter>
                    
                </AppShellFooter>
            </AppShell>  
        </AppProvider>
    );
}