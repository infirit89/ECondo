import { AppShell, AppShellFooter, AppShellHeader, AppShellMain } from "@mantine/core";
import { ReactNode } from "react";
import { Navbar } from "./navbar";
import { isAuthenticated } from "@/actions/auth";
import { getBriefProfile } from "@/actions/profile";
import { ProfileCreationModal } from "./profileCreationModal";
import { BriefProfileResponse } from "@/types/profileData";
import { UserNavbar } from "./userNavbar";

export default async function App({children} : {children: ReactNode}) {
    let openProfileCreationModal = false;
    const authenticated = await isAuthenticated();
    let profileData: BriefProfileResponse | null = null;
    if(authenticated) {
        try {
            profileData = await getBriefProfile();
        }
        catch(error) {
            openProfileCreationModal = true;
        }
    }
    
    return (
        <AppShell>
            <ProfileCreationModal opened={openProfileCreationModal}/>
            <AppShellHeader>
                { authenticated ? 
                <UserNavbar 
                username={profileData?.username!} 
                firstName={profileData?.firstName!} 
                lastName={profileData?.lastName!}/> 
                : <Navbar/> } 
            </AppShellHeader>
            <AppShellMain mt={100}>
                {children}
            </AppShellMain>
            <AppShellFooter>
                
            </AppShellFooter>
        </AppShell>
    );
}
