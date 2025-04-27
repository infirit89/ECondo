import Footer from "@/components/footer";
import Loading from "@/components/loading";
import { UserNavbar } from "@/components/navbar/userNavbar";
import { AppShell, AppShellMain } from "@mantine/core";
import { ReactNode, Suspense } from "react";

export default function CommonUserLayout(
    { children }: Readonly<{ children: ReactNode }> ) {

    return (
        <AppShell 
        header={{ height: 60 }}>
            <UserNavbar />
            <AppShellMain>
                <Suspense fallback={<Loading/>}>
                    {children}
                </Suspense>
            </AppShellMain>
            <Footer/>
        </AppShell>
    );
}
