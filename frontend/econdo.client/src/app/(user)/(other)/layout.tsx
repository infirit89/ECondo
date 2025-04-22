import { UserNavbar } from "@/components/navbar/userNavbar";
import { AppShell, AppShellFooter, AppShellMain } from "@mantine/core";
import { ReactNode } from "react";

export default function CommonUserLayout(
    { children }: Readonly<{ children: ReactNode }> ) {

    return (
        <AppShell 
        header={{ height: 60 }}>
            <UserNavbar />
            <AppShellMain>
                {children}
            </AppShellMain>
            {/* <AppShellFooter> */}
                
            {/* </AppShellFooter> */}
        </AppShell>
    );
}