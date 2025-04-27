import Footer from "@/components/footer";
import { Navbar } from "@/components/navbar/navbar";
import { AppShell, AppShellMain } from "@mantine/core";

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
            <Footer/>
        </AppShell>
    );
}
