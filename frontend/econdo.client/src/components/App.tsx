import { AppShell, AppShellFooter, AppShellHeader, AppShellMain } from "@mantine/core";
import { ReactNode } from "react";
import { Navbar } from "./navbar";

export default function App({children} : {children: ReactNode}) {
    return (
        <AppShell>
            <AppShellHeader>
                <Navbar/>
            </AppShellHeader>
            <AppShellMain mt={100}>
                {children}
            </AppShellMain>
            <AppShellFooter>
                
            </AppShellFooter>
        </AppShell>
    )
}