import { AppShell, AppShellFooter, AppShellHeader, AppShellMain } from "@mantine/core";
import { ReactNode } from "react";

export default function App({children} : {children: ReactNode}) {
    return (
        <AppShell>
            <AppShellHeader>

            </AppShellHeader>
            <AppShellMain>
                {children}
            </AppShellMain>
            <AppShellFooter>
                
            </AppShellFooter>
        </AppShell>
    )
}