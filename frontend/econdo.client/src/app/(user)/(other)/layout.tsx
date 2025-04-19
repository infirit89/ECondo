'use server';

import { getBriefProfile } from "@/actions/profile";
import { UserNavbar } from "@/components/navbar/userNavbar";
import { AppShell, AppShellFooter, AppShellMain } from "@mantine/core";
import { ReactNode, Suspense } from "react";

export default async function CommonUserLayout( { children }: 
    Readonly<{ children: ReactNode }> ) {

    const profileRes = await getBriefProfile();

    if(!profileRes.ok)
        throw new Error();

    return (
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
    );
}