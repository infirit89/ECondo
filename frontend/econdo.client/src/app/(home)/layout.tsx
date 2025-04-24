import { Navbar } from "@/components/navbar/navbar";
import { AppShell, AppShellFooter, AppShellMain } from "@mantine/core";

export default function AuthLayout({ children }:  Readonly<{
    children: React.ReactNode;
  }>) {
    return (
        <AppShell 
        header={{ height: 60 }}>
            <Navbar/>
            <AppShellMain>
                {children}
            </AppShellMain>
            <AppShellFooter>
                
            </AppShellFooter>
        </AppShell>
    );
}