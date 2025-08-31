'use client';

import Footer from "@/components/footer";
import { UserNavbar } from "@/components/navbar/userNavbar";
import { AppShell, AppShellMain } from "@mantine/core";
import { ReactNode } from "react";
import DashboardSidebar from "./dashboardSidebar";
import { usePathname } from "next/navigation";

// TODO: consider:
// 1. moving the building and property pages to be a part of the condos/building and condos/buildings respectively
// 2. fix any and all fetch errors related to the new authorization system and the fact that entrance based queries no longer take in a building id and entrance number
// they take in only the entrance id for improved simplicity
export default function AdminLayout({ children }:
    Readonly<{
        children: ReactNode,
    }>) {

    const pathName = usePathname();

    return (
        <>Under construction</>
    );
    // return (
    //     <AppShell
    //         header={{ height: 60 }}
    //         navbar={{ width: 300, breakpoint: 'sm', collapsed: { mobile: true } }}>
    //         <UserNavbar />
    //         <DashboardSidebar
    //             activeTab={pathName} />
    //         <AppShellMain>
    //             {children}
    //         </AppShellMain>
    //         <Footer />
    //     </AppShell>
    // );
}
