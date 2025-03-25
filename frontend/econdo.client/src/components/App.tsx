'use client';

import { AppShell, AppShellFooter, AppShellHeader, AppShellNavbarConfiguration } from "@mantine/core";
import { ReactNode } from "react";
import { Navbar } from "./navbar";
import { ProfileCreationModal } from "./profileCreationModal";
import { UserNavbar } from "./userNavbar";;
import { BriefProfileResponse } from "@/types/profileData";
import { usePathname } from "next/navigation";

export default function App({isAuthenticated, profileData, children} : {isAuthenticated: boolean, profileData?: BriefProfileResponse, children: ReactNode}) {
    const pathname = usePathname();
    const navbarConfig: AppShellNavbarConfiguration | undefined = 
        isAuthenticated && pathname !== '/condos' ? { width: 300, breakpoint: 'sm', collapsed: { mobile: true } }
        : undefined;
    
    return (
        <AppShell 
        header={{ height: 60 }}
        navbar={navbarConfig}>
            <ProfileCreationModal opened={!profileData && isAuthenticated}/>
            <AppShellHeader>
                { isAuthenticated ? 
                <UserNavbar 
                username={profileData?.username!} 
                firstName={profileData?.firstName} 
                lastName={profileData?.lastName}/> 
                : <Navbar/> } 
            </AppShellHeader>
            {children}
            <AppShellFooter>
                
            </AppShellFooter>
        </AppShell>
    );
}
