import { AppShell, AppShellFooter, AppShellHeader, AppShellMain, AppShellNavbar, AppShellNavbarConfiguration } from "@mantine/core";
import { ReactNode } from "react";
import { Navbar } from "./navbar";
import { ProfileCreationModal } from "./profileCreationModal";
import { UserNavbar } from "./userNavbar";
import DashboardNavbar from "./dashboardNavbar";
import { BriefProfileResponse } from "@/types/profileData";

export default function App({isAuthenticated, profileData, children} : {isAuthenticated: boolean, profileData?: BriefProfileResponse, children: ReactNode}) {
    const navbarConfig: AppShellNavbarConfiguration | undefined = 
        isAuthenticated ? { width: 300, breakpoint: 'sm', collapsed: { mobile: true } }
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
                firstName={profileData?.firstName!} 
                lastName={profileData?.lastName!}/> 
                : <Navbar/> } 
            </AppShellHeader>
            {
                isAuthenticated ?
                <AppShellNavbar>
                    <DashboardNavbar/>
                </AppShellNavbar>
                : <></>
            }
            <AppShellMain>
                {children}
            </AppShellMain>
            <AppShellFooter>
                
            </AppShellFooter>
        </AppShell>
    );
}
