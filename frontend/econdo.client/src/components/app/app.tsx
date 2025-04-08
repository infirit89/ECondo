'use client';

import { AppShell, AppShellFooter, AppShellHeader, AppShellNavbarConfiguration } from "@mantine/core";
import { ReactNode } from "react";
import { Navbar } from "../navbar/navbar";
import { ProfileCreationModal } from "../profile/profileCreationModal";
import { UserNavbar } from "../navbar/userNavbar";;
import { BriefProfileResponse } from "@/types/profileData";
import { usePathname } from "next/navigation";

const dashboardSeperatePaths = [ '/condos', '/profile' ];

export default function App({isAuthenticated, profileData, children} : {isAuthenticated: boolean, profileData?: BriefProfileResponse, children: ReactNode}) {
    const pathname = usePathname();
    const navbarConfig: AppShellNavbarConfiguration | undefined = 
        isAuthenticated && !dashboardSeperatePaths.includes(pathname) 
        ? { width: 300, breakpoint: 'sm', collapsed: { mobile: true } }
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
