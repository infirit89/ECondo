'use server';

import { ReactNode } from "react";
import { ProfileCreationModal } from "../profile/profileCreationModal";
import { BriefProfileResponse } from "@/types/profileData";

export default async function AppProvider(
    { profileData, children } : 
    { profileData: BriefProfileResponse | undefined, 
        children: ReactNode }) {

    return (
        <>
            <ProfileCreationModal opened={ !profileData }/>
            { profileData ?
                children :
                <></>}
        </>
    );
}
