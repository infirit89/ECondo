'use client';

import Footer from "@/components/footer";
import { UserNavbar } from "@/components/navbar/userNavbar";
import { AppShell, AppShellMain } from "@mantine/core";
import { ReactNode } from "react";
import DashboardSidebar from "./dashboardSidebar";
import { usePathname } from "next/navigation";

export default function AdminLayout({ children }: 
    Readonly<{
        children: ReactNode, }>) {
    
    const pathName = usePathname();
    
    return (
    <AppShell 
    header={{ height: 60 }}
    navbar={{ width: 300, breakpoint: 'sm', collapsed: { mobile: true }} }>
        <UserNavbar/>
        <DashboardSidebar 
        activeTab={pathName}/>
        <AppShellMain>
            { children }
        </AppShellMain>
        <Footer/>
    </AppShell>
    );
}